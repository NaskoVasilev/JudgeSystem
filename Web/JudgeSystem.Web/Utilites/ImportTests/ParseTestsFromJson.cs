using System;
using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Services;
using JudgeSystem.Web.InputModels.Problem;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public class ParseTestsFromJson : ParseTestsStrategy
    {
        public override IEnumerable<ProblemTestInputModel> Parse(IServiceProvider serviceProvider, IFormFile file, ICollection<string> errorMessages)
        {
            if (string.IsNullOrEmpty(file.FileName) || !file.FileName.EndsWith(GlobalConstants.JsonFileExtension))
            {
                errorMessages.Add(string.Format(ErrorMessages.InvalidFileExtension, GlobalConstants.JsonFileExtension));
                return Enumerable.Empty<ProblemTestInputModel>();
            }

            IHostingEnvironment env = serviceProvider.GetRequiredService<IHostingEnvironment>();
            IJsonUtiltyService jsonUtiltyService = serviceProvider.GetRequiredService<IJsonUtiltyService>();

            using System.IO.Stream stream = file.OpenReadStream();
            string schemaFilePath = env.WebRootPath + GlobalConstants.AddTestsInputJsonFileSchema;
            return jsonUtiltyService.ParseJsonFormStreamUsingJSchema<List<ProblemTestInputModel>>(stream, schemaFilePath, errorMessages.ToList());
        }
    }
}
