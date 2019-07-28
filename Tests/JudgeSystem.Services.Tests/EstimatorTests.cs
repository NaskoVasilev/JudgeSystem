using System;
using Xunit;

namespace JudgeSystem.Services.Tests
{
    public class EstimatorTests
    {
        private readonly IEstimator estimator;

        public EstimatorTests()
        {
            this.estimator = new Estimator();
        }

        [Fact]
        public void CalculateRemainingTime_WithEndTimeInTheFuture_ShouldWorkCorrect()
        {
            DateTime endTime = DateTime.Now.AddDays(5).AddHours(2).AddMinutes(45).AddSeconds(12);

            var remainingTime = estimator.CalculateRemainingTime(endTime);

            Assert.Equal(5, remainingTime.Days);
            Assert.Equal(2, remainingTime.Hours);
            Assert.Equal(45, remainingTime.Minutes);
            Assert.True(remainingTime.Seconds <= 12);
        }

        [Fact]
        public void CalculateRemainingTime_WithEndTimeInThePast_AllValuesMustBeZero()
        {
            DateTime endTime = DateTime.Now.AddDays(-5);

            var remainingTime = estimator.CalculateRemainingTime(endTime);

            Assert.Equal(0, remainingTime.Days);
            Assert.Equal(0, remainingTime.Hours);
            Assert.Equal(0, remainingTime.Minutes);
            Assert.Equal(0, remainingTime.Seconds);
        }

        [Theory]
        [InlineData(0, 10, 100, 0)]
        [InlineData(10, 0, 100, 0)]
        [InlineData(10, 5, 100, 50)]
        [InlineData(9, 4, 70, 31)]
        [InlineData(9, 5, 70, 38)]
        public void CalculteProblemPoints_WithDifferntInputs_ShouldReturnDifferentData(int testsCount, int passedTests,
            int maxPoints, int expectedPoints)
        {
            int actualPoints = estimator.CalculteProblemPoints(testsCount, passedTests, maxPoints);

            Assert.Equal(expectedPoints, actualPoints);
        }
    }
}
