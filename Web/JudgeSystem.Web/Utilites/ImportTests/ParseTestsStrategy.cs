using System;
using System.Collections.Generic;

using JudgeSystem.Web.InputModels.Problem;

using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public abstract class ParseTestsStrategy
    {
        public abstract IEnumerable<ProblemTestInputModel> Parse(IServiceProvider serviceProvider, IFormFile file, ICollection<string> errorMessages);
    }
}
