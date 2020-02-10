using System.Collections.Generic;
using JudgeSystem.Common;
using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public class ParseTestsFromJson<T> : ParseTestsStrategy<T>
    {
        public override IEnumerable<T> Parse(IFormFile file, ICollection<string> errorMessages)
        {
            var messages = new List<string>();
            using System.IO.Stream stream = file.OpenReadStream();
            string schemaFilePath = env.WebRootPath + GlobalConstants.AddTestsInputJsonFileSchema;
            List<T> tests = jsonUtiltyService.ParseJsonFormStreamUsingJSchema<List<T>>(stream, schemaFilePath, messages);

        }
    }
}
