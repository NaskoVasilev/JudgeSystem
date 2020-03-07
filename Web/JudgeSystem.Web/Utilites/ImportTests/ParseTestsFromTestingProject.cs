using System;
using System.Collections.Generic;

using JudgeSystem.Services;
using JudgeSystem.Web.InputModels.Problem;
using static JudgeSystem.Common.GlobalConstants;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public class ParseTestsFromTestingProject : ParseTestsStrategy
    {
        public override IEnumerable<ProblemTestInputModel> Parse(IServiceProvider serviceProvider, IFormFile file, ICollection<string> errorMessages)
        {
            //write all file with extensions: .cs and .csproj in temp directory
            IUtilityService utilityService = serviceProvider.GetRequiredService<IUtilityService>();
            utilityService.ParseZip(file.OpenReadStream(), new HashSet<string> { CSharpProjectExtension, CSharpFileExtension });

            //run command: dotnet test --list-tests
            //check output
            //if there are some errors shiw them to use
            //else split output by new line and then create tests with testname as input and "Passed" as output
            //save file in the database
            //return response that the tests are imported

            return null;
        }
    }
}
