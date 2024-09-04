using System.Text;

namespace OnAim.Admin.APP.Helpers
{
    public static class CsvExportHelper
    {
        public static string ExportToCsv<T>(IEnumerable<T> records, IEnumerable<string> columns)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine(string.Join(",", columns));

            foreach (var record in records)
            {
                var values = columns.Select(column =>
                {
                    var property = record.GetType().GetProperty(column);
                    return property?.GetValue(record, null);
                });

                csvBuilder.AppendLine(string.Join(",", values));
            }

            return csvBuilder.ToString();
        }
    }
}
