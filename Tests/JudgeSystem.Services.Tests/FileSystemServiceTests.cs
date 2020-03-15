using System.IO;

using Xunit;

namespace JudgeSystem.Services.Tests
{
    public class FileSystemServiceTests
    {
        private readonly FileSystemService fileSystem;

        public FileSystemServiceTests()
        {
            fileSystem = new FileSystemService();
        }

        [Fact]
        public void DeleteDirectory_WithExistingPath_ShouldDeleteTheDirectoryAndAllFilesInIt()
        {
            string directoryName = "testDirectory";
            DirectoryInfo directoryInfo = Directory.CreateDirectory(directoryName);
            File.WriteAllText(Path.Combine(directoryInfo.FullName, "test.txt"), "This is test file with should be deleted!");

            fileSystem.DeleteDirectory(directoryName);

            Assert.False(Directory.Exists(directoryInfo.FullName));
        }
    }
}
