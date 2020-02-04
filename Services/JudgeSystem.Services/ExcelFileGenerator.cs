using GemBox.Spreadsheet;
using System.IO;

namespace JudgeSystem.Services
{
    public class ExcelFileGenerator : IExcelFileGenerator
    {
        private const string LicenseKey = "FREE-LIMITED-KEY";

        public byte[] Generate(string[] columns, string[,] data)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            XlsxSaveOptions options = SaveOptions.XlsxDefault;
            var workbook = new ExcelFile();
            ExcelWorksheet worksheet = workbook.Worksheets.Add("Sheet1");

            CellStyle style = worksheet.Rows[0].Style;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 0; i < columns.Length; i++)
            {
                worksheet.Cells[0, i].Value = columns[i];
            }

            for (int row = 0; row < data.GetLength(0); row++)
            {
                for (int col = 0; col < data.GetLength(1); col++)
                {
                    worksheet.Cells[row + 1, col].Value = data[row, col];
                }
            }

            return GetBytes(workbook, options);
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
