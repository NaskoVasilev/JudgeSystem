using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using JudgeSystem.Services;
using JudgeSystem.Web.InputModels.Problem;
using JudgeSystem.Services.Models;
using static JudgeSystem.Common.GlobalConstants;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public class ParseTestsFromTestingProject : ParseTestsStrategy
    {
        public const int UnnecessaryLines = 5;

        public override IEnumerable<ProblemTestInputModel> Parse(IServiceProvider serviceProvider, IFormFile file, ICollection<string> errorMessages)
        {
            IFileSystemService fileSystem = serviceProvider.GetRequiredService<IFileSystemService>();
            IProcessRunner processRunner = serviceProvider.GetRequiredService<IProcessRunner>();

            //write all file with extensions: .cs and .csproj in temp directory
            ProcessResult processResult;
            string projectDirectory = string.Empty;

            try
            {
                projectDirectory = fileSystem.CreateDirectory(CompilationDirectoryPath, Path.GetRandomFileName());
                string filePath = Path.Combine(projectDirectory, file.FileName);
                fileSystem.CreateFile(file.OpenReadStream(), filePath).GetAwaiter().GetResult();
                fileSystem.ExtractZipToDirectory(filePath, projectDirectory);

                //run command: dotnet test --list-tests
                string command = processRunner.PrependChangeDirectoryCommand(ProcessRunner.DotnetListTestsCommand, projectDirectory);
                processResult = processRunner.Run(command, projectDirectory).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                fileSystem.DeleteDirectory(projectDirectory);
            }


            if (!processResult.IsSuccessfull)
            {
                errorMessages.Add(processResult.Errors);
                yield break;
            }

            IEnumerable<string> testNames = processResult.Output.Trim().Split(Environment.NewLine).Skip(UnnecessaryLines);
            foreach (string testName in testNames)
            {
                yield return new ProblemTestInputModel
                {
                    InputData = testName.Trim(),
                    OutputData = PassedTest
                };
            }
        }
    }
}
