using System;
using System.Collections.Generic;

using JudgeSystem.Common;
using JudgeSystem.Services.Models;
using JudgeSystem.Web.Dtos.Test;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Services
{
    public class Checker : IChecker
    {
        public CheckerResult Check(ExecutionResult executionResult, string expectedOutput)
        {
            var checkerResult = new CheckerResult(executionResult);
            if (!executionResult.IsSuccesfull)
            {
                checkerResult.IsCorrect = false;
                return checkerResult;
            }

            if (executionResult.Output == expectedOutput)
            {
                checkerResult.IsCorrect = true;
            }
            else
            {
                checkerResult.IsCorrect = false;
            }

            return checkerResult;
        }

        public IEnumerable<AutomatedTestResult> CheckAutomatedTestsOutput(IEnumerable<TestDataDto> tests, ProcessResult testResults, string projectDirectory)
        {
            var testIndexByTestName = new Dictionary<int, string>();
            var faildTestsByError = new Dictionary<string, string>();
            testResults.Output = testResults.Output.Replace(projectDirectory, string.Empty);
            int failMessagesEndIndex = testResults.Output.LastIndexOf($"{Environment.NewLine}{Environment.NewLine}");
            if (failMessagesEndIndex != -1 && !testResults.IsSuccessfull)
            {
                var testIndicies = new List<int>
                {
                    failMessagesEndIndex
                };

                foreach (TestDataDto test in tests)
                {
                    int index = testResults.Output.LastIndexOf(test.InputData);
                    if (index != -1)
                    {
                        testIndicies.Add(index);
                        testIndexByTestName[index] = test.InputData;
                    }
                }

                testIndicies.Sort();

                for (int i = 0; i < testIndicies.Count - 1; i++)
                {
                    int startIndex = testIndicies[i];
                    int endIndex = testIndicies[i + 1];
                    string error = testResults.Output.Substring(startIndex, endIndex - startIndex);
                    string testName = testIndexByTestName[startIndex];
                    faildTestsByError[testName] = error;
                }
            }

            foreach (TestDataDto test in tests)
            {
                bool isCorrect = true;
                string testOuput = GlobalConstants.PassedTest;
                if (faildTestsByError.TryGetValue(test.InputData, out string testError))
                {
                    testOuput = testError;
                    isCorrect = false;
                }

                yield return new AutomatedTestResult(testOuput, isCorrect, test);
            }
        }
    }
}
