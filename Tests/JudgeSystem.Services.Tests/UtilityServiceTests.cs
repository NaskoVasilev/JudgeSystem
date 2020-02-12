using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Web.Dtos.Common;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Workers.Common;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Xunit;

namespace JudgeSystem.Services.Tests
{
    public class UtilityServiceTests
    {
        private readonly UtilityService utilityService;

        public UtilityServiceTests()
        {
            utilityService = new UtilityService();
        }

        [Theory]
        [InlineData(1000000, 1)]
        [InlineData(508124000, 508.124)]
        [InlineData(50000, 0.05)]
        [InlineData(0, 0)]
        public void ConvertBytesToMegaBytes_WithDiferentArguments_ShouldWorkCorrect(long bytes, double expectedMegaBytes)
        {
            double actualMegaBytes = utilityService.ConvertBytesToMegaBytes(bytes);

            Assert.Equal(expectedMegaBytes, actualMegaBytes);
        }

        [Theory]
        [InlineData(750, 0.75)]
        [InlineData(1000, 1)]
        [InlineData(5489, 5.489)]
        [InlineData(0, 0)]
        public void ConvertBytesToKiloBytes_WithDiferentArguments_ShouldWorkCorrect(long bytes, double expectedKiloBytes)
        {
            double actualKiloBytes = utilityService.ConvertBytesToKiloBytes(bytes);

            Assert.Equal(expectedKiloBytes, actualKiloBytes);
        }


        [Theory]
        [InlineData(0.1500258478, 150025)]
        [InlineData(3.154843302, 3154843)]
        [InlineData(1, 1000000)]
        [InlineData(0, 0)]
        public void ConvertMegaBytesToBytes_WithDiferentArguments_ShouldWorkCorrect(double megaBytes, int expectedBytes)
        {
            int actualBytes = utilityService.ConvertMegaBytesToBytes(megaBytes);

            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        public async Task ExtractSubmissionCode_WithNotNullValidCodeAndNoFile_ShouldReturnCorrectsubmissionCodeDto()
        {
            string code = "using System;\r\nclass Test\r\n{\r\n}\r\n";
            SubmissionCodeDto submissionDto = await utilityService.ExtractSubmissionCode(code, null, ProgrammingLanguage.CSharp);

            Assert.Equal(Encoding.UTF8.GetBytes(code), submissionDto.Content);
            Assert.Equal(submissionDto.SourceCodes.Select(x => x.Code), new List<string>() { code });
            Assert.False(string.IsNullOrEmpty(submissionDto.SourceCodes.First().Name));
        }

        [Fact]
        public async Task ExtractSubmissionCode_WithToBigCode_ShouldThrowBadrequestException()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < GlobalConstants.MaxSubmissionCodeLength + 1; i++)
            {
                sb.Append("a");
            }

            await Assert.ThrowsAsync<BadRequestException>(() => utilityService.ExtractSubmissionCode(sb.ToString(), null, ProgrammingLanguage.CSharp));
        }

        [Fact]
        public async Task ExtractSubmissionCode_WithNullCodeArgumentAndValidFile_ShouldReturnCorrectsubmissionCodeDto()
        {
            string firstClass = "using System;\r\nclass Test\r\n{\r\n}\r\n";
            string secondClass = "//smaple solution comes here\r\nusing System;\r\nclass Program\r\n{\r\n}\r\n\t";

            using (FileStream stream = File.OpenRead(ServiceTestsConstants.TestDataFolderPath + "/ZippedSolution.zip"))
            {
                IFormFile file = new FormFile(stream, 0, stream.Length, "solution", "solution.zip");
                SubmissionCodeDto submissionDto = await utilityService.ExtractSubmissionCode(null, file, ProgrammingLanguage.CSharp);

                using (var ms = new MemoryStream())
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(ms);
                    Assert.Equal(ms.ToArray(), submissionDto.Content);
                }

                Assert.Equal(2, submissionDto.SourceCodes.Count);
                var sourceCodes = submissionDto.SourceCodes.OrderBy(x => x.Code.Length).ToList();
                Assert.Equal(firstClass, sourceCodes[0].Code);
                Assert.Equal(secondClass, sourceCodes[1].Code);
                Assert.Equal("solution", sourceCodes[0].Name);
                Assert.Equal("sampleSolution", sourceCodes[1].Name);
            }
        }

        [Fact]
        public async Task ExtractSubmissionCode_WithNullCodeArgumentAndTooBigFile_ShouldThrowBadRequestException()
        {
            var random = new Random(0);
            int bytesCount = (GlobalConstants.SubmissionFileMaxSizeInKb * 1000) + 10;
            byte[] buffer = new byte[bytesCount];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)random.Next(256);
            }

            using (var stream = new MemoryStream(buffer))
            {
                IFormFile file = new FormFile(stream, 0, stream.Length, "solution", "solution.zip");
                await Assert.ThrowsAsync<BadRequestException>(() => utilityService.ExtractSubmissionCode(null, file, ProgrammingLanguage.CSharp));
            }
        }

        [Fact]
        public async Task ExtractSubmissionCode_WithNullCodeArgumentAndNullFileArgumet_ShouldThrowBadRequestException() =>
            await Assert.ThrowsAsync<BadRequestException>(() => utilityService.ExtractSubmissionCode(null, null, ProgrammingLanguage.Java));

        [Fact]
        public async Task ExtractSubmissionCode_WithNotNullCodeArgumentAndNotNullFileArgumet_ShouldGetSubmissionCodeFromCodeArgument()
        {
            IFormFile file = new FormFile(new MemoryStream(), 0, 0, "solution", "solution.zip");
            string code = "task code comes here";
            SubmissionCodeDto submissionDto = await utilityService.ExtractSubmissionCode(code, file, ProgrammingLanguage.CPlusPlus);

            Assert.Equal(Encoding.UTF8.GetBytes(code), submissionDto.Content);
            Assert.Equal(submissionDto.SourceCodes.Select(x => x.Code), new List<string>() { code });
        }

        [Fact]
        public void ExtractZipFile_WithNestedFoldersAndForbiddenExtensions_ShouldExtractOnlyFilesWithAllowedExtensions()
        {
            string firstFileData = "Some test data comes here";
            string secondFileData = "using System;\r\nclass Test\r\n{\r\n}\r\n";
            string thirdFileData = "let sum = (a, b) => a + b;\r\nlet multiply = (a, b) => a * b;";
            string fourthFileData = "//smaple solution comes here\r\nusing System;\r\nclass Program\r\n{\r\n}\r\n\t";

            using (FileStream stream = File.OpenRead(ServiceTestsConstants.ZippedSolutionPath))
            {
                List<CodeFile> sourceCodes = utilityService.ExtractZipFile(stream, new HashSet<string> { ".txt", ".cs", ".js" });

                sourceCodes = sourceCodes.OrderBy(x => x.Code.Length).ToList();
                Assert.Equal(4, sourceCodes.Count);
                Assert.Equal(firstFileData, sourceCodes[0].Code);
                Assert.Equal(secondFileData, sourceCodes[1].Code);
                Assert.Equal(thirdFileData, sourceCodes[2].Code);
                Assert.Equal(fourthFileData, sourceCodes[3].Code);
            }
        }

        [Fact]
        public void ParseZip_WithPassedZipFile_ShouldReturnAllFilesInTheZip()
        {
            //Arrange
            var fileNames = new List<string>
            {
                "test.txt",
                "code.js",
                "phpSolution.php",
                "sampleSolution.cs",
                "solutionJava.java",
                "cppSolution.cpp",
                "solution.cs"
            }
            .OrderBy(x => x);

            using (FileStream stream = File.OpenRead(ServiceTestsConstants.ZippedSolutionPath))
            {
                //Act
                IEnumerable<string> files = utilityService.ParseZip(stream)
                    .Select(x => x.Name)
                    .OrderBy(x => x);

                //Assert
                Assert.Equal(fileNames, files);
            }
        }

        [Fact]
        public void ParseZip_WithPassedZipFile_ShouldReadContentForeachFile()
        {
            //Arrange
            string testContent = "Some test data comes here";
            string fileName = "test.txt";

            using (FileStream stream = File.OpenRead(ServiceTestsConstants.ZippedSolutionPath))
            {
                //Act
                IEnumerable<FileDto> files = utilityService.ParseZip(stream, new HashSet<string> { ".txt" });

                //Assert
                string content = files.First(x => x.Name == fileName).Content;
                Assert.Equal(testContent, content);
            }
        }

        [Fact]
        public void ParseZip_WithPassedZipFileAndAllowedFileExtensions_ShouldReturnOnlyMatchingFiles()
        {
            //Arrange
            var fileNames = new List<string>
            {
                "code.js",
                "sampleSolution.cs",
                "solutionJava.java",
                "cppSolution.cpp",
                "solution.cs"
            }
            .OrderBy(x => x);

            using (FileStream stream = File.OpenRead(ServiceTestsConstants.ZippedSolutionPath))
            {
                //Act
                IEnumerable<string> files = utilityService.ParseZip(stream, new HashSet<string>() { ".cs", ".java", ".cpp", ".in", ".js" })
                    .Select(x => x.Name)
                    .OrderBy(x => x);

                //Assert
                Assert.Equal(fileNames, files);
            }
        }

        [Fact]
        public void DeleteDirectory_WithExistingPath_ShouldDeleteTheDirectoryAndAllFilesInIt()
        {
            string directoryName = "testDirectory";
            DirectoryInfo directoryInfo = Directory.CreateDirectory(directoryName);
            File.WriteAllText(Path.Combine(directoryInfo.FullName, "test.txt"), "This is test file with should be deleted!");

            utilityService.DeleteDirectory(directoryName);

            Assert.False(Directory.Exists(directoryInfo.FullName));
        }

        [Theory]
        [InlineData("static void main(String[] args){\n")]
        [InlineData("static void main (String[] startUpArgs) {\n")]
        [InlineData("static void main()")]
        public void GetJavaMainClass_WithValidCode_ShouldExtractOnlyTheJavaClassName(string code)
        {
            string actualName = utilityService.GetJavaMainClass(new List<string> { code });

            Assert.Equal(actualName, code);
        }

        [Theory]
        [InlineData("static void mai()")]
        public void GetJavaMainClass_WithInvalidCode_ShouldReturnNull(string code)
        {
            string actualName = utilityService.GetJavaMainClass(new List<string> { code });

            Assert.Null(actualName);
        }

        [Theory]
        [InlineData("class Test{\n", "Test")]
        [InlineData("class Test123 {\n", "Test123")]
        [InlineData("class Test123", "Test123")]
        public void GetJavaClassName_WithValidCode_ShouldExtractOnlyTheJavaClassName(string code, string expectedName)
        {
            string actualName = utilityService.GetJavaClassName(code);

            Assert.Equal(expectedName, actualName);
        }

        [Theory]
        [InlineData("clas Test")]
        public void GetJavaClassName_WithInvalidCode_ShouldThrowBadRequestException(string code) =>
            Assert.Throws<BadRequestException>(() => utilityService.GetJavaClassName(code));
    }
}
