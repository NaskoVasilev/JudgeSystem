using System;
using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Services;
using JudgeSystem.Web.InputModels.Problem;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public class ParseTestsFromZip : ParseTestsStrategy
    {
        public override IEnumerable<ProblemTestInputModel> Parse(IServiceProvider serviceProvider, IFormFile file, ICollection<string> errorMessages)
        {
            IUtilityService utilityService = serviceProvider.GetRequiredService<IUtilityService>();
            var inputAndOutputs = utilityService.ParseZip(file.OpenReadStream()).ToList();

            for (int i = 0; i < inputAndOutputs.Count - 1; i += 2)
            {
                string input = inputAndOutputs[i];
                string output = inputAndOutputs[i + 1];
                yield return new ProblemTestInputModel()
                {
                    InputData = input,
                    OutputData = output
                };
            }
        }
    }
}
}
