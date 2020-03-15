using System.Collections.Generic;
using System;

namespace JudgeSystem.Services
{
    public class CodeCompareer : ICodeCompareer
    {
        public double GetMinCodeDifference(string sourceCode, IEnumerable<string> otherCodes)
        {
            double minCodeDistance = double.MaxValue;
            string minifiedSourceCode = MinifyString(sourceCode);

            foreach (string otherCode in otherCodes)
            {
                string minifiedOtherCode = MinifyString(otherCode);
                double codeDistance = GetStringDistanceInPercentages(minifiedSourceCode, minifiedOtherCode);
                if(codeDistance < minCodeDistance)
                {
                    minCodeDistance = codeDistance;
                }
            }

            return minCodeDistance;
        }

        private double GetStringDistanceInPercentages(string sourceString, string otherString)
        {
            int stringDistance = GetStringDistance(sourceString, otherString);
            int stringLength = Math.Max(sourceString.Length, otherString.Length);
            return (double)stringDistance / stringLength * 100;
        }

        private string MinifyString(string str)
        {
            string[] stringParts = str.Split(new char[] { '\r', '\n', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(string.Empty, stringParts);
        }

        private int GetStringDistance(string first, string second)
        {
            var bounds = new { Rows = first.Length + 1, Cols = second.Length + 1 };

            int[,] matrix = new int[bounds.Rows, bounds.Cols];

            for (int row = 0; row < bounds.Rows; row++)
            {
                matrix[row, 0] = row;
            }

            for (int col = 0; col < bounds.Cols; col++)
            {
                matrix[0, col] = col;
            }

            for (int row = 1; row < bounds.Rows; row++)
            {
                for (int col = 1; col < bounds.Cols; col++)
                {
                    int cost = (first[row - 1] == second[col - 1]) ? 0 : 1;
                    int insertion = matrix[row, col - 1] + 1;
                    int deletion = matrix[row - 1, col] + 1;
                    int substitution = matrix[row - 1, col - 1] + cost;

                    int distance = Math.Min(insertion, Math.Min(deletion, substitution));

                    if (row > 1 && col > 1 && first[row - 1] == second[col - 2] && first[row - 2] == second[col - 1])
                    {
                        distance = Math.Min(distance, matrix[row - 2, col - 2] + cost);
                    }

                    matrix[row, col] = distance;
                }
            }

            return matrix[bounds.Rows - 1, bounds.Cols - 1];
        }
    }
}
