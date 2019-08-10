using System;

using JudgeSystem.Common;

using Xunit;

namespace JudgeSystem.Services.Tests
{
    public class ValidationServiceTests
    {
        private readonly ValidationService validationService;

        public ValidationServiceTests()
        {
            validationService = new ValidationService();
        }

        [Fact]
        public void IsValidFileExtension_WithValidFileNames_ShoudReturnTrue()
        {
            foreach (var fileExtension in GlobalConstants.AllowedFileExtensoins)
            {
                string fileName = $"{Guid.NewGuid()}{fileExtension}";

                Assert.True(validationService.IsValidFileExtension(fileName));
            }
        }

        [Theory]
        [InlineData("name.txt.bat")]
        [InlineData("name.bat")]
        [InlineData("name.exe")]
        [InlineData("withoutExtension")]
        [InlineData("name.exe.bat.txt.dll")]
        public void IsValidFileExtension_WithInvalidFileNames_ShoudReturnFalse(string fileName)
        {
            Assert.False(validationService.IsValidFileExtension(fileName));
        }
    }
}
