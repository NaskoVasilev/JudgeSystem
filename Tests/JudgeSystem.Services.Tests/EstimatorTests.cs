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
    }
}
