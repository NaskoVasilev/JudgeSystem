using System.IO;

using JudgeSystem.Common;

namespace JudgeSystem.Web.Configuration
{
    public static class CompilersConfiguration
    {
        public static void CreateWorkingDirectoryIfNotExists()
        {
            string workingDirectory = GlobalConstants.CompilationDirectoryPath;
            if (Directory.Exists(workingDirectory))
            {
                return;
            }

            Directory.CreateDirectory(workingDirectory);
        }
    }
}
