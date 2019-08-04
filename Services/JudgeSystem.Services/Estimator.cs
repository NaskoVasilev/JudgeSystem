using System;

using JudgeSystem.Web.Dtos.Common;

namespace JudgeSystem.Services
{
	public class Estimator : IEstimator
	{
		public TimeRemainingDto CalculateRemainingTime(DateTime endTime)
		{
            if(endTime < DateTime.Now)
            {
                return new TimeRemainingDto();
            }

			TimeSpan difference = endTime - DateTime.Now;
			TimeRemainingDto timeRemaining = new TimeRemainingDto
			{
				Days = difference.Days,
				Hours = difference.Hours,
				Minutes = difference.Minutes,
				Seconds = difference.Seconds
			};
			return timeRemaining;
		}

		public int CalculteProblemPoints(int testsCount, int passedTests, int maxPoints)
		{
            if(testsCount == 0 || maxPoints == 0)
            {
                return 0;
            }

			double pointsPerTest = (double)maxPoints / testsCount;
			double actualPoints = passedTests * pointsPerTest;
			return (int)actualPoints;
		}
	}
}
