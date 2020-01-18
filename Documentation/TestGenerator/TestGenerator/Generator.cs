using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace TestGenerator
{
    public class Generator
    {
        //Path of the output file
        private const string OutputFilePath = @"../../../Data/tests.json";
        //Path of the input file
        private const string InputFilePath = @"../../../Data/inputs.txt";

        //Do not touch anything in Main method just change the code in ExecuteTest method
        public static void Main(string[] args)
        {
            //Read all inputs form some file
            string allInputsAsString = File.ReadAllText(InputFilePath);
            //Two new lines identicate end of test. So delimiter each test with two new lines
            string[] inputs = allInputsAsString.Split($"{Environment.NewLine}{Environment.NewLine}");

            var tests = new List<Test>();

            foreach (string input in inputs)
            {
                //We will sotre the output in this string builder. 
                //Use the Trahsformer.cs to replace Console.Write with lines.Append and Console.WriteLine with lines.AppendLine in target code
                var lines = new StringBuilder();
                int index = 0;
                //In this array we store the data for each test. 
                //So user the Transformer.cs to replace each Console.ReadLine() with data[index++] in target code
                string[] data = input.Split(Environment.NewLine);

                //Paste the solution of the task in this method afetr transform it using the Transformer.cs
                ExecuteTest(lines, data, index);

                //Make test object from the input and output
                tests.Add(new Test
                {
                    InputData = input,
                    OutputData = lines.ToString().TrimEnd(),
                    IsTrialTest = false
                });
            }

            //Convetrt all tests to json and write the result in the output file
            string json = JsonConvert.SerializeObject(tests, Formatting.Indented);
            File.WriteAllText(OutputFilePath, json);
        }

        private static void ExecuteTest(StringBuilder lines, string[] data, int index)
        {
            //Solution of the task comes here. So paste transformed code here.
            //Use the Tramsformer.cs to replace Console.Write with lines.Append, Console.WriteLine with lines.AppendLine and 
            //Console.ReadLine() with data[index++] before paste the solution of the task here
            int n = int.Parse(data[index++]);
            int k = int.Parse(data[index++]);
            int[] nums = new int[n];
            int sum = 0;
            nums[0] = 1;
            for (int i = 1; i < n; i++)
            {
                int initialPlace;
                if (i - k < 0)
                {
                    initialPlace = 0;
                }
                else
                {
                    initialPlace = i - k;
                }
                for (int j = initialPlace; j < i; j++)
                {
                    sum += nums[j];
                }
                nums[i] = sum;
                sum = 0;
            }
            lines.AppendLine(string.Join(" ", nums));
        }
    }


}
