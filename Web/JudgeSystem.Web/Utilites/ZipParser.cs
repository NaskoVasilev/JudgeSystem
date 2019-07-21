using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace JudgeSystem.Web.Utilites
{
    public static class ZipParser
    {
        private const string CSharpFileExtension = ".cs";

        public static List<string> ExtractZipFile(Stream stream)
        {
            List<string> filesData = new List<string>();

            using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (entry.Name.EndsWith(CSharpFileExtension))
                    {
                        using (StreamReader reader = new StreamReader(entry.Open()))
                        {
                            string data = reader.ReadToEnd();
                            filesData.Add(data);
                        }
                    }
                }
            }

            return filesData;
        }
    }
}
