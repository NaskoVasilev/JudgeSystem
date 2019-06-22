using System;
using JudgeSystem.Web.Dtos.Common;

namespace JudgeSystem.Services
{
	public class Estimator : IEstimator
	{
		private const int HoursInDay = 24;

		public TimeRemainingDto CalculateRemainingTime(DateTime endTime)
		{
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
			double pointsPerTest = (double)maxPoints / testsCount;
			double actualPoints = passedTests * pointsPerTest;
			return (int)actualPoints;
		}
	}
}
