﻿using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Data.Models;
using JudgeSystem.Web.Dtos.Problem;
using JudgeSystem.Web.InputModels.Problem;
using JudgeSystem.Web.ViewModels.Problem;
using JudgeSystem.Web.ViewModels.Search;

namespace JudgeSystem.Services.Data
{
	public  interface IProblemService
	{
		Task<ProblemDto> Create(ProblemInputModel model);

		IEnumerable<LessonProblemViewModel> LessonProblems(int lessonId);

		Task<TDestination> GetById<TDestination>(int id);

		Task<ProblemDto> Update(ProblemEditInputModel model);

		Task<ProblemDto> Delete(int id);

		IEnumerable<SearchProblemViewModel> SearchByName(string keyword);

		Task<int> GetLessonId(int problemId);

        string GetProblemName(int id);

        int GetTimeIntevalBetweenSubmissionInSeconds(int problemId);

        Task AddAutomatedTestingProject(int id, byte[] testingProject);

        Task<byte[]> GetAutomatedTestingProject(int id);
    }
}
