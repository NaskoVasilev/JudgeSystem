using System;

using Xunit;

namespace JudgeSystem.Services.Tests
{
    public class PaginationServiceTests
    {
        private readonly PaginationService paginationService;

        public PaginationServiceTests()
        {
            paginationService = new PaginationService();
        }

        [Theory]
        [InlineData(10, 5, 2)]
        [InlineData(10, 6, 2)]
        [InlineData(10, 4, 3)]
        [InlineData(0, 4, 0)]
        [InlineData(1, 4, 1)]
        public void CalculatePagesCount_WithDifferentData_ShouldReturnCorrectValues(int elelentsCount, int elementsPerPage, int expectedPages)
        {
            int actualPages = paginationService.CalculatePagesCount(elelentsCount, elementsPerPage);

            Assert.Equal(expectedPages, actualPages);
        }

        [Fact]
        public void CalculatePagesCount_WithNegativeElementsPerPage_ShouldThrowError() => 
            Assert.Throws<ArgumentException>(() => paginationService.CalculatePagesCount(10, -20));

        [Fact]
        public void CalculatePagesCount_WithZeroElementsPerPage_ShouldThrowError() => 
            Assert.Throws<ArgumentException>(() => paginationService.CalculatePagesCount(10, 0));

        [Fact]
        public void CalculatePagesCount_WithNegativeElementsCount_ShouldThrowError() => 
            Assert.Throws<ArgumentException>(() => paginationService.CalculatePagesCount(10, -1));
    }
}
