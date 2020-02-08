using Xunit;

namespace JudgeSystem.Services.Tests
{
    public class StringFormatterTests
    {
        [Fact]
        public void FormatPoints_WithActualPointsAndMaxPointsAsParameters_ShouldReturnCorrectlyFormattedString()
        {
            int actualPoints = 10;
            int maxPoints = 100;
            var formatter = new StringFormatter();

            string result = formatter.FormatPoints(actualPoints, maxPoints);

            Assert.Equal($"{actualPoints}/{maxPoints}", result);
        }
    }
}
