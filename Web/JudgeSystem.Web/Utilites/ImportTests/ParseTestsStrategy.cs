using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public abstract class ParseTestsStrategy<T>
    {
        public abstract IEnumerable<T> Parse(IFormFile file, ICollection<string> errorMessages);
    }
}
