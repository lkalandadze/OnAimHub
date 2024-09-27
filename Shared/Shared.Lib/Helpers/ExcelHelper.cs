using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Shared.Lib.Helpers;

public static class ExcelHelper
{
    public static async Task<List<T>> ReadExcelFirstColumnAsync<T>(this IFormFile file)
    {
        return await file.ReadExcelColumnAsync<T>(0);
    }

    public static async Task<List<T>> ReadExcelColumnAsync<T>(this IFormFile file, int columnIndex)
    {
        var result = new List<T>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);

            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var cellValue = worksheet.Cells[row, columnIndex + 1].Text;

                try
                {

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        var convertedValue = ConvertHelper.ConvertValue<T>(cellValue);
                        result.Add(convertedValue);
                    }
                }
                catch (Exception ex)
                {
                    throw new FormatException($"Failed to convert [{cellValue}] (column {columnIndex + 1}, row {row}) as {TypeHelper.TypeName<T>(cellValue)}", ex);
                }
            }
        }

        return result;
    }
}