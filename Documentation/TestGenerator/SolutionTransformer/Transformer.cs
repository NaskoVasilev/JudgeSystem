using System.IO;

namespace SolutionTransformer
{
    public class Transformer
    {
        public static void Main(string[] args)
        {
            string filePath = "../../../Code/Code.cs";
            string code = File.ReadAllText(filePath);

            string[][] replacePairs = new string[][]
            {
                new string[] { "Console.ReadLine()", "data[index++]" },
                new string[] { "Console.WriteLine", "lines.AppendLine" },
                new string[] { "Console.Write", "lines.Append" },
            };
            code = TransformCode(code, replacePairs);

            File.WriteAllText(filePath, code);
        }

        private static string TransformCode(string code, string[][] replacePairs)
        {
            foreach (string[] replacePair in replacePairs)
            {
                string oldValue = replacePair[0];
                string newValue = replacePair[1];

                code = code.Replace(oldValue, newValue);
            }

            return code;
        }
    }
}
