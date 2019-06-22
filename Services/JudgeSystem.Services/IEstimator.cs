using JudgeSystem.Web.Dtos.Common;
using System;

namespace JudgeSystem.Services
{
	public interface IEstimator
	{
		int CalculteProblemPoints(int testsCount, int passedTests, int maxPoints);

		TimeRemainingDto CalculateRemainingTime(DateTime endTime);
	}
}
