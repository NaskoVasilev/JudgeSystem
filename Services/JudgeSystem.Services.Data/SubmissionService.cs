using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JudgeSystem.Checkers;
using JudgeSystem.Common;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Compilers;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.ExecutedTest;
using JudgeSystem.Web.Dtos.Problem;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Web.Dtos.Test;
using JudgeSystem.Web.InputModels.Submission;
using JudgeSystem.Web.ViewModels.Submission;
using JudgeSystem.Workers.Common;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IRepository<Submission> repository;
        private readonly IEstimator estimator;
        private readonly IProblemService problemService;
        private readonly ITestService testService;
        private readonly IExecutedTestService executedTestService;
        private readonly IUtilityService utilityService;

        public SubmissionService(
            IRepository<Submission> repository,
            IEstimator estimator,
            IProblemService problemService,
            ITestService testService,
            IExecutedTestService executedTestService,
            IUtilityService utilityService)
        {
            this.repository = repository;
            this.estimator = estimator;
            this.problemService = problemService;
            this.testService = testService;
            this.executedTestService = executedTestService;
            this.utilityService = utilityService;
        }

        public async Task<SubmissionDto> Create(SubmissionInputModel model, string userId)
        {
            if (!model.PracticeId.HasValue && !model.ContestId.HasValue)
            {
                throw new BadRequestException(ErrorMessages.InvalidSubmissionSource);
            }

            if (model.ContestId.HasValue)
            {
                model.PracticeId = null;
            }

            var submission = new Submission
            {
                Code = model.SubmissionContent,
                ProblemId = model.ProblemId,
                UserId = userId,
                ContestId = model.ContestId,
                PracticeId = model.PracticeId
            };

            await repository.AddAsync(submission);
            return submission.To<SubmissionDto>();
        }

        public SubmissionResult GetSubmissionResult(int id)
        {
            SubmissionResult submissioResult = repository.All()
                .Where(s => s.Id == id)
                .Include(s => s.ExecutedTests)
                .Include(s => s.Problem)
                .Select(MapSubmissionToSubmissionResult)
                .FirstOrDefault();

            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(submissioResult, nameof(Submission));

            return submissioResult;
        }

        public IEnumerable<SubmissionResult> GetUserSubmissionsByProblemId(int problemId, string userId, int page, int submissionsPerPage)
        {
            IQueryable<Submission> submissionsFromDb = repository.All()
                .Where(s => s.ProblemId == problemId && s.UserId == userId);

            IEnumerable<SubmissionResult> submissions = GetSubmissionResults(submissionsFromDb, page, submissionsPerPage);
            return submissions;
        }

        public int GetProblemSubmissionsCount(int problemId, string userId) =>
            repository.All().Count(s => s.ProblemId == problemId && s.UserId == userId);

        public int GetSubmissionsCountByProblemIdAndContestId(int problemId, int contestId, string userId) =>
            repository.All().Count(s => s.ProblemId == problemId && s.UserId == userId && s.ContestId == contestId);

        public SubmissionViewModel GetSubmissionDetails(int id)
        {
            SubmissionViewModel submission = repository.All()
                .Where(s => s.Id == id)
                .To<SubmissionViewModel>()
                .FirstOrDefault();

            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(submission, nameof(Submission));

            submission.ExecutedTests = submission.ExecutedTests.OrderByDescending(t => t.TestIsTrialTest).ToList();
            return submission;
        }

        public IEnumerable<SubmissionResult> GetUserSubmissionsByProblemIdAndContestId(int contestId, int problemId, string userId, int page, int submissionsPerPage)
        {
            IQueryable<Submission> submissionsFromDb = repository.All()
                .Where(s => s.ContestId == contestId && s.UserId == userId && s.ProblemId == problemId);

            IEnumerable<SubmissionResult> submissions = GetSubmissionResults(submissionsFromDb, page, submissionsPerPage);
            return submissions;
        }

        public async Task CalculateActualPoints(int submissionId)
        {
            Submission submission = repository.All()
                .Where(s => s.Id == submissionId)
                .Include(s => s.Problem)
                .Include(s => s.ExecutedTests)
                .ThenInclude(e => e.Test)
                .FirstOrDefault();

            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(submission, nameof(Submission));

            if (submission.CompilationErrors != null && submission.CompilationErrors.Length > 0)
            {
                return;
            }

            int passedTests = submission.ExecutedTests.Count(t => t.IsCorrect && !t.Test.IsTrialTest);
            int executedTests = submission.ExecutedTests.Count(t => !t.Test.IsTrialTest);
            int maxPoints = submission.Problem.MaxPoints;

            if (passedTests == 0 || executedTests == 0)
            {
                submission.ActualPoints = 0;
            }
            else
            {
                submission.ActualPoints = estimator.CalculteProblemPoints(executedTests, passedTests, maxPoints);
            }

            await repository.UpdateAsync(submission);
        }

        public byte[] GetSubmissionCodeById(int id)
        {
            byte[] code = repository.All()
                .Where(x => x.Id == id)
                .Select(x => x.Code)
                .FirstOrDefault();

            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(code, nameof(Submission));

            return code;
        }

        public string GetProblemNameBySubmissionId(int id)
        {
            string problemName = repository.All()
                .Include(x => x.Problem)
                .Where(x => x.Id == id)
                .Select(x => x.Problem.Name)
                .FirstOrDefault();

            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(problemName, nameof(Submission));

            return problemName;
        }

        public IEnumerable<SubmissionResult> GetUserSubmissionsByProblemIdAndPracticeId(int practiceId, int problemId, string userId, int page, int submissionsPerPage)
        {
            IQueryable<Submission> submissionsFromDb = repository.All()
                .Where(s => s.PracticeId == practiceId && s.UserId == userId && s.ProblemId == problemId);

            IEnumerable<SubmissionResult> submissions = GetSubmissionResults(submissionsFromDb, page, submissionsPerPage);
            return submissions;
        }

        public async Task ExecuteSubmission(int submissionId, List<string> sourceCodes)
        {
            Submission submission = await repository.FindAsync(submissionId);

            var compiler = new CSharpCompiler();
            string fileName = Path.GetRandomFileName();
            string workingDirectory = $"{GlobalConstants.CompilationDirectoryPath}{Guid.NewGuid().ToString()}\\";
            CompileResult compileResult = compiler.Compile(fileName, workingDirectory, sourceCodes);

            if (!compileResult.IsCompiledSuccessfully)
            {
                submission.CompilationErrors = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, compileResult.Errors));
                await Update(submission);
            }
            else
            {
                await RunTests(submission, compileResult);
                Directory.Delete(workingDirectory);
            }
        }

        public int GetSubmissionsCountByProblemIdAndPracticeId(int problemId, int practiceId, string userId) => 
            repository.All().Count(s => s.ProblemId == problemId && s.UserId == userId && s.PracticeId == practiceId);

        private SubmissionResult MapSubmissionToSubmissionResult(Submission submission)
        {
            var submissionResult = new SubmissionResult
            {
                MaxPoints = submission.Problem.MaxPoints,
                ActualPoints = submission.ActualPoints,
                ExecutedTests = submission.ExecutedTests.Select(t => new ExecutedTestResult
                {
                    IsCorrect = t.IsCorrect,
                    ExecutionResultType = t.ExecutionResultType.ToString()
                })
                .ToList(),
                IsCompiledSuccessfully = submission.CompilationErrors == null || submission.CompilationErrors.Length == 0,
                SubmissionDate = submission.SubmisionDate.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture),
                Id = submission.Id,
                TotalMemoryUsed = submission.ExecutedTests.Sum(t => t.MemoryUsed),
                TotalTimeUsed = submission.ExecutedTests.Sum(t => t.TimeUsed)
            };

            return submissionResult;
        }

        private IEnumerable<SubmissionResult> GetSubmissionResults(IQueryable<Submission> submissionsFromDb, int page, int submissionsPerPage)
        {
            var submissions = submissionsFromDb
                .Include(s => s.ExecutedTests)
                .Include(s => s.Problem)
                .OrderByDescending(s => s.SubmisionDate)
                .Skip((page - 1) * submissionsPerPage)
                .Take(submissionsPerPage)
                .Select(MapSubmissionToSubmissionResult)
                .ToList();

            return submissions;
        }

        private async Task Update(Submission submission)
        {
            ThrowEntityNotFoundExceptionIfEntityDoesNotExists(submission.Id);

            await repository.UpdateAsync(submission);
        }

        private bool Exists(int id) => repository.All().Any(x => x.Id == id);

        private void ThrowEntityNotFoundExceptionIfEntityDoesNotExists(int id)
        {
            if (!Exists(id))
            {
                throw new EntityNotFoundException(nameof(Submission));
            }
        }

        private async Task RunTests(Submission submission, CompileResult compileResult)
        {
            var tests = testService.GetTestsByProblemId(submission.ProblemId).ToList();
            ProblemConstraintsDto problemConstraints = await problemService.GetById<ProblemConstraintsDto>(submission.ProblemId);
            if (tests.Count == 0)
            {
                return;
            }

            var checker = new CSharpChecker();
            int memoryLimit = utilityService.ConvertMegaBytesToBytes(problemConstraints.AllowedMemoryInMegaBytes) / tests.Count;
            int timeLimit = problemConstraints.AllowedTimeInMilliseconds / tests.Count;

            foreach (TestDataDto test in tests)
            {
                CheckerResult checkerResult = await checker.Check(compileResult.OutputFilePath, test.InputData, test.OutputData, timeLimit, memoryLimit);
                Enum.TryParse<TestExecutionResultType>(checkerResult.Type.ToString(), out TestExecutionResultType executionResultType);

                var executedTest = new ExecutedTest
                {
                    TestId = test.Id,
                    SubmissionId = submission.Id,
                    ExecutionResultType = executionResultType,
                    TimeUsed = checkerResult.TotalProcessorTime.TotalSeconds,
                    Output = checkerResult.Output,
                    Error = checkerResult.Error,
                    MemoryUsed = utilityService.ConvertBytesToMegaBytes(checkerResult.MemoryUsed),
                    IsCorrect = checkerResult.Type == ProcessExecutionResultType.Success && checkerResult.IsCorrect
                };

                await executedTestService.Create(executedTest);
            }
        }
    }
}
