using System.IO;
using System.Threading.Tasks;

namespace JudgeSystem.Common.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ToArray(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (stream)
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public static async Task<string> ReadToEndAsync(this Stream stream) 
        {
            using (var streamReader = new StreamReader(stream))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}
