using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Web.Infrastructure.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> ToArrayAsync(this IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
