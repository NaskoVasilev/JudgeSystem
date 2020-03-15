using System.IO;

namespace JudgeSystem.Common.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ToArray(this Stream stream)
        {
            using var memoryStream = new MemoryStream();
            using (stream)
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
