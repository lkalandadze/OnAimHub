using ClosedXML.Excel;

namespace OnAim.Admin.Shared.Helpers
{
    public static class ExcelExportHelper
    {
        public static byte[] ExportToExcel<T>(IEnumerable<T> records, IEnumerable<string> columns)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            for (int i = 0; i < columns.Count(); i++)
            {
                worksheet.Cell(1, i + 1).Value = columns.ElementAt(i);
            }

            int rowIndex = 2;
            foreach (var record in records)
            {
                for (int i = 0; i < columns.Count(); i++)
                {
                    var property = record.GetType().GetProperty(columns.ElementAt(i));
                    if (property != null)
                    {
                        var value = property.GetValue(record, null);

                        if (value is IEnumerable<object> list)
                        {
                            var names = list.Select(item =>
                            {
                                var itemType = item.GetType();
                                var nameProperty = itemType.GetProperty("Name");
                                return nameProperty != null ? nameProperty.GetValue(item, null)?.ToString() : string.Empty;
                            });
                            value = string.Join(", ", names);
                        }
                        worksheet.Cell(rowIndex, i + 1).Value = value != null ? value.ToString() : string.Empty;
                    }
                }
                rowIndex++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
