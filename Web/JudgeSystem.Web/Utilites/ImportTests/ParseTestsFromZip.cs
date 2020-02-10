using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public class ParseTestsFromZip<T> : ParseTestsStrategy<T>
    {
        public override IEnumerable<T> Parse(IFormFile file, ICollection<string> errorMessages)
        {
            throw new System.NotImplementedException();
        }
    }
}
