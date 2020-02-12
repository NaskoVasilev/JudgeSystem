using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JudgeSystem.Common;
using JudgeSystem.Services;
using JudgeSystem.Web.InputModels.Problem;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public class ParseTestsFromZip : ParseTestsStrategy
    {
        private const string TrialTestPattern = @"\.0{2,}\.";

        public override IEnumerable<ProblemTestInputModel> Parse(IServiceProvider serviceProvider, IFormFile file, ICollection<string> errorMessages)
        {
            if (string.IsNullOrEmpty(file.FileName) || !file.FileName.EndsWith(GlobalConstants.ZipFileExtension))
            {
                errorMessages.Add(string.Format(ErrorMessages.InvalidFileExtension, GlobalConstants.ZipFileExtension));
                yield break;
            }
            
            IUtilityService utilityService = serviceProvider.GetRequiredService<IUtilityService>();
            var inputAndOutputs = utilityService.ParseZip(file.OpenReadStream()).ToList();
            var regex = new Regex(TrialTestPattern);

            for (int i = 0; i < inputAndOutputs.Count - 1; i += 2)
            {
                string input = inputAndOutputs[i].Content;
                string output = inputAndOutputs[i + 1].Content;
                string testName = inputAndOutputs[i].Name;
                yield return new ProblemTestInputModel()
                {
                    InputData = input,
                    OutputData = output,
                    IsTrialTest = regex.IsMatch(testName)
                };
            }
        }
    }
}

