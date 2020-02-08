using System;
using System.IO;
using System.Collections.Generic;

using JudgeSystem.Web.ViewModels.Contest;
using JudgeSystem.Web.ViewModels.Problem;
using JudgeSystem.Web.ViewModels.Practice;

using GemBox.Spreadsheet;

namespace JudgeSystem.Services
{
    public class ExcelFileGenerator : IExcelFileGenerator
    {
        private const string LicenseKey = "FREE-LIMITED-KEY";
        private const string DefaultWorksheet = "Sheet1";
        private const int CellPadding = 5;

        private readonly IStringFormatter stringFormatter;

        public ExcelFileGenerator(IStringFormatter stringFormatter)
        {
            this.stringFormatter = stringFormatter;
        }

        public byte[] GenerateContestResultsReport(ContestAllResultsViewModel contestResultsData, List<string> columns)
        {
            Action<ExcelWorksheet> fillWorksheet = (ExcelWorksheet worksheet) =>
            {
                worksheet.Columns[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                SetHeadRowStyle(worksheet, HorizontalAlignmentStyle.Center);
                AddColumns(worksheet, columns);
                worksheet.Columns[2].SetWidth(200, LengthUnit.Pixel);

                for (int row = 1; row <= contestResultsData.ContestResults.Count; row++)
                {
                    int col = 0;
                    ContestResultViewModel contestResult = contestResultsData.ContestResults[row - 1];
                    worksheet.Cells[row, col++].Value = contestResult.Student.NumberInCalss;
                    worksheet.Cells[row, col++].Value = contestResult.Student.ClassName;
                    worksheet.Cells[row, col++].Value = contestResult.Student.FullName;

                    foreach (ContestProblemViewModel problem in contestResultsData.Problems)
                    {
                        worksheet.Cells[row, col++].Value = stringFormatter.FormatPoints(contestResult.GetPoints(problem.Id), problem.MaxPoints);
                    }

                    worksheet.Cells[row, col++].Value = stringFormatter.FormatPoints(contestResult.Total, contestResultsData.MaxPoints);
                }
            };

            return Generate(fillWorksheet);
        }

        public byte[] GeneratePracticeResultsReport(PracticeAllResultsViewModel practiceResultsData, List<string> columns)
        {
            Action<ExcelWorksheet> fillWorksheet = (ExcelWorksheet worksheet) =>
            {
                SetHeadRowStyle(worksheet, HorizontalAlignmentStyle.Center);
                AddColumns(worksheet, columns);
                worksheet.Columns[0].SetWidth(200, LengthUnit.Pixel);

                for (int row = 1; row <= practiceResultsData.PracticeResults.Count; row++)
                {
                    int col = 0;
                    PracticeResultViewModel practiceResult = practiceResultsData.PracticeResults[row - 1];
                    worksheet.Cells[row, col++].Value = practiceResult.FullName;
                    worksheet.Cells[row, col++].Value = practiceResult.Username;

                    foreach (PracticeProblemViewModel problem in practiceResultsData.Problems)
                    {
                        worksheet.Cells[row, col++].Value = stringFormatter.FormatPoints(practiceResult.GetPoints(problem.Id), problem.MaxPoints);
                    }

                    worksheet.Cells[row, col++].Value = stringFormatter.FormatPoints(practiceResult.Total, practiceResultsData.MaxPoints);
                }
            };

            return Generate(fillWorksheet);
        }

        public byte[] Generate(List<string> columns, object[,] data)
        {
            Action<ExcelWorksheet> fillWorksheet = (ExcelWorksheet worksheet) =>
            {
                SetHeadRowStyle(worksheet, HorizontalAlignmentStyle.Center);
                AddColumns(worksheet, columns);

                for (int row = 0; row < data.GetLength(0); row++)
                {
                    for (int col = 0; col < data.GetLength(1); col++)
                    {
                        worksheet.Cells[row + 1, col].Value = data[row, col];
                    }
                }
            };

            return Generate(fillWorksheet);
        }

        private byte[] Generate(Action<ExcelWorksheet> fillWorksheet, string worksheetName = DefaultWorksheet)
        {
            SpreadsheetInfo.SetLicense(LicenseKey);
            XlsxSaveOptions options = SaveOptions.XlsxDefault;
            var workbook = new ExcelFile();
            ExcelWorksheet worksheet = workbook.Worksheets.Add(worksheetName);

            fillWorksheet(worksheet);

            return GetBytes(workbook, options);
        }

        private void AddColumns(ExcelWorksheet worksheet, List<string> columns)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                worksheet.Cells[0, i].Value = columns[i];
                worksheet.Columns[i].SetWidth(columns[i].Length + CellPadding, LengthUnit.ZeroCharacterWidth);
            }
        }

        private static void SetHeadRowStyle(ExcelWorksheet worksheet, HorizontalAlignmentStyle alignmentStyle, int fontWeight = ExcelFont.BoldWeight) => 
            SetRowStyle(worksheet, 0, alignmentStyle, fontWeight);

        private static void SetRowStyle(ExcelWorksheet worksheet, int row, HorizontalAlignmentStyle alignmentStyle, int fontWeight = ExcelFont.NormalWeight)
        {
            CellStyle style = worksheet.Rows[row].Style;
            style.Font.Weight = fontWeight;
            style.HorizontalAlignment = alignmentStyle;
        }

        private void AlignColumns(ExcelWorksheet worksheet, int startCol, int endCol, HorizontalAlignmentStyle alignmentStyle)
        {
            for (int col = startCol; col <= endCol; col++)
            {
                worksheet.Columns[col].Style.HorizontalAlignment = alignmentStyle;
            }
        }

        private static byte[] GetBytes(ExcelFile file, SaveOptions options)
        {
            using (var stream = new MemoryStream())
            {
                file.Save(stream, options);
                return stream.ToArray();
            }
        }
    }
}
