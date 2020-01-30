using JudgeSystem.Workers.Common;

using Xunit;

namespace JudgeSystem.Services.Tests
{
    public class CheckerTests
    {
        [Fact]
        public void Check_WithUnsucessfullResult_ShouldSetIsCorrectToTrue()
        {
            var checker = new Checker();
            var executionResult = new ExecutionResult() { Error = "error" };

            CheckerResult checkerResult = checker.Check(executionResult, null);

            Assert.False(checkerResult.IsCorrect);
        }

        [Theory]
        [InlineData("test", "test", true)]
        [InlineData("test ", "test ", true)]
        [InlineData("test\r\n", "test\r\n", true)]
        [InlineData("test ", "test", false)]
        [InlineData("test   ", "test ", false)]
        [InlineData("test\r\n", "test", false)]
        public void Check_WithDifferentArguments_ShouldreturnDifferentResults(string output, string expectedOutput, bool isCorrect)
        {
            var checker = new Checker();
            var executionResult = new ExecutionResult() { Output = output };

            CheckerResult checkerResult = checker.Check(executionResult, expectedOutput);

            Assert.Equal(isCorrect, checkerResult.IsCorrect);
        }
    }
}
