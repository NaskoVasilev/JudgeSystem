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
            double worldRecordTime = double.Parse(data[index++]);
            double distanceToBeSwum = double.Parse(data[index++]);
            double timeForSwimming1m = double.Parse(data[index++]);

            double resistanceTime = ((int)distanceToBeSwum / 15) * 12.5;
            double totalTime = (distanceToBeSwum * timeForSwimming1m) + resistanceTime;

            if (totalTime < worldRecordTime)
            {
                lines.AppendLine($"Yes, he succeeded! The new world record is {totalTime:F2} seconds.");
            }
            else
            {
                lines.AppendLine($"No, he failed! He was {totalTime - worldRecordTime:F2} seconds slower.");
            }
        }
    }


}
