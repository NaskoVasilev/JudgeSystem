using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Compilers;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Executors;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.ExecutedTest;
using JudgeSystem.Web.Dtos.Problem;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Web.Dtos.Test;
using JudgeSystem.Web.InputModels.Submission;
using JudgeSystem.Web.ViewModels.Submission;
using JudgeSystem.Workers.Common;
using static JudgeSystem.Common.GlobalConstants;
using JudgeSystem.Services.Models;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IDeletableEntityRepository<Submission> repository;
        private readonly IEstimator estimator;
        private readonly IProblemService problemService;
        private readonly ITestService testService;
        private readonly IExecutedTestService executedTestService;
        private readonly IUtilityService utilityService;
        private readonly IFileSystemService fileSystem;
        private readonly ICompilerFactory compilerFactory;
        private readonly IExecutorFactory executorFactory;
        private readonly IChecker checker;
        private readonly IProcessRunner processRunner;

        public SubmissionService(
            IDeletableEntityRepository<Submission> repository,
            IEstimator estimator,
            IProblemService problemService,
            ITestService testService,
            IExecutedTestService executedTestService,
            IUtilityService utilityService,
            IFileSystemService fileSystem,
            ICompilerFactory compilerFactory,
            IExecutorFactory executorFactory,
            IChecker checker,
            IProcessRunner processRunner)
        {
            this.repository = repository;
            this.estimator = estimator;
            this.problemService = problemService;
            this.testService = testService;
            this.executedTestService = executedTestService;
            this.utilityService = utilityService;
            this.fileSystem = fileSystem;
            this.compilerFactory = compilerFactory;
            this.executorFactory = executorFactory;
            this.checker = checker;
            this.processRunner = processRunner;
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

        public async Task ExecuteSubmission(int submissionId, List<CodeFile> codeFiles, ProgrammingLanguage programmingLanguage)
        {
            Submission submission = await repository.FindAsync(submissionId);
            var sourceCodes = codeFiles.Select(x => x.Code).ToList();
            string fileName = codeFiles.First().Name;
            if (programmingLanguage == ProgrammingLanguage.Java)
            {
                fileName = GetJavaFileName(sourceCodes);
                if (codeFiles.Count == 1)
                {
                    codeFiles.First().Name = fileName;
                }
            }

            string workingDirectory = $"{GlobalConstants.CompilationDirectoryPath}{Path.GetRandomFileName()}/";
            Directory.CreateDirectory(workingDirectory);

            //Created directory above will be deleted even if some of the code below throws exception beacuse we use finally block
            try
            {
                ICompiler compiler = compilerFactory.CreateCompiler(programmingLanguage);
                CompileResult compileResult;

                if (programmingLanguage != ProgrammingLanguage.CSharp)
                {
                    utilityService.CreateLanguageSpecificFiles(programmingLanguage, codeFiles, workingDirectory);
                    compileResult = compiler.Compile(fileName, workingDirectory);
                }
                else
                {
                    compileResult = compiler.Compile(fileName, workingDirectory, sourceCodes);
                }

                if (!compileResult.IsCompiledSuccessfully)
                {
                    submission.CompilationErrors = Encoding.UTF8.GetBytes(compileResult.Errors);
                    await Update(submission);
                }
                else
                {
                    await RunTests(submission, compileResult, programmingLanguage);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                fileSystem.DeleteDirectory(workingDirectory);
            }
        }

        public int GetSubmissionsCountByProblemIdAndPracticeId(int problemId, int practiceId, string userId) =>
            repository.All().Count(s => s.ProblemId == problemId && s.UserId == userId && s.PracticeId == practiceId);

        public async Task DeleteSubmissionsByProblemId(int problemId)
        {
            var submissions = repository.All().Where(x => x.ProblemId == problemId).ToList();
            await repository.DeleteRangeAsync(submissions);
        }

        public async Task RunAutomatedTests(int id, ProgrammingLanguage programmingLanguage)
        {
            Submission submission = await repository.FindAsync(id);
            byte[] testingProject = await problemService.GetAutomatedTestingProject(submission.ProblemId);
            ProblemConstraintsDto problemConstraints = await problemService.GetById<ProblemConstraintsDto>(submission.ProblemId);
            string projectDirectory = string.Empty;

            try
            {
                projectDirectory = fileSystem.CreateDirectory(CompilationDirectoryPath, Path.GetRandomFileName());
                string testingProjectFilePath = Path.Combine(projectDirectory, Path.GetRandomFileName() + ZipFileExtension);
                await fileSystem.CreateFile(testingProject, testingProjectFilePath);
                fileSystem.ExtractZipToDirectory(testingProjectFilePath, projectDirectory);

                string userProjectFilePath = Path.Combine(projectDirectory, Path.GetRandomFileName() + ZipFileExtension);
                await fileSystem.CreateFile(submission.Code, userProjectFilePath);
                fileSystem.ExtractZipToDirectory(userProjectFilePath, projectDirectory, overwrite: true);

                string buildCommand = processRunner.PrependChangeDirectoryCommand(ProcessRunner.DotnetBuildProjectCommand, projectDirectory);
                ProcessResult buildProjectResult = await processRunner.Run(buildCommand, projectDirectory);
                if (buildProjectResult.Output.ToLower().Contains(ProcessRunner.BuildFaildMessage.ToLower()))
                {
                    buildProjectResult.Errors = buildProjectResult.Output.Replace(projectDirectory, string.Empty);
                    submission.CompilationErrors = Encoding.UTF8.GetBytes(buildProjectResult.Errors);
                    await Update(submission);
                }
                else
                {
                    var tests = testService.GetTestsByProblemIdOrderedByIsTrialDescending<TestDataDto>(submission.ProblemId).ToList();
                    ProcessResult testResults = await processRunner.Run(ProcessRunner.DotnetRunTestsCommand, projectDirectory);
                    IEnumerable<AutomatedTestResult> automatedTestsResults = checker.CheckAutomatedTestsOutput(tests, testResults, projectDirectory);

                    foreach (AutomatedTestResult automatedTestResult in automatedTestsResults)
                    {
                        var executedTest = new ExecutedTest
                        {
                            TestId = automatedTestResult.TestData.Id,
                            SubmissionId = submission.Id,
                            ExecutionResultType = TestExecutionResultType.Success,
                            TimeUsed = problemConstraints.AllowedTimeInMilliseconds,
                            Output = automatedTestResult.Ouput,
                            MemoryUsed = problemConstraints.AllowedMemoryInMegaBytes,
                            IsCorrect = automatedTestResult.IsCorrect
                        };

                        await executedTestService.Create(executedTest);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                fileSystem.DeleteDirectory(projectDirectory);
            }
        }

        private string GetJavaFileName(List<string> sourceCodes)
        {
            string javaMainClass = utilityService.GetJavaMainClass(sourceCodes);

            if (string.IsNullOrEmpty(javaMainClass))
            {
                throw new BadRequestException(ErrorMessages.InvalidMainJavaClass);
            }

            return utilityService.GetJavaClassName(javaMainClass);
        }

        private SubmissionResult MapSubmissionToSubmissionResult(Submission submission)
        {
            var submissionResult = new SubmissionResult
            {
                MaxPoints = submission.Problem.MaxPoints,
                ActualPoints = submission.ActualPoints,
                ExecutedTests = submission.ExecutedTests
                .OrderBy(x => x.CreatedOn)
                .Select(t => new ExecutedTestResult
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
                .Where(x => x.ExecutedTests.Count > 0 || x.CompilationErrors != null)
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

        private async Task RunTests(Submission submission, CompileResult compileResult, ProgrammingLanguage programmingLanguage)
        {
            var tests = testService.GetTestsByProblemIdOrderedByIsTrialDescending<TestDataDto>(submission.ProblemId).ToList();
            ProblemConstraintsDto problemConstraints = await problemService.GetById<ProblemConstraintsDto>(submission.ProblemId);
            if (tests.Count == 0)
            {
                return;
            }

            int memoryLimit = utilityService.ConvertMegaBytesToBytes(problemConstraints.AllowedMemoryInMegaBytes);
            int timeLimit = problemConstraints.AllowedTimeInMilliseconds;

            IExecutor executor = executorFactory.CreateExecutor(programmingLanguage);

            foreach (TestDataDto test in tests)
            {
                ExecutionResult executionResult = await executor.Execute(compileResult.OutputFilePath, test.InputData, timeLimit, memoryLimit);
                CheckerResult checkerResult = checker.Check(executionResult, test.OutputData);
                Enum.TryParse(checkerResult.Type.ToString(), out TestExecutionResultType executionResultType);

                var executedTest = new ExecutedTest
                {
                    TestId = test.Id,
                    SubmissionId = submission.Id,
                    ExecutionResultType = executionResultType,
                    TimeUsed = checkerResult.TimeWorked.TotalSeconds,
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
