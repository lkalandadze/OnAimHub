namespace OnAim.Admin.Shared.Helpers
{
    public class AuditLogger
    {
        public static string GenerateChangeLog<T>(T oldEntity, T newEntity)
        {
            var changes = new List<string>();

            foreach (var property in typeof(T).GetProperties())
            {
                var oldValue = property.GetValue(oldEntity);
                var newValue = property.GetValue(newEntity);

                if (!Equals(oldValue, newValue))
                {
                    changes.Add($"{property.Name} changed from '{oldValue}' to '{newValue}'");
                }
            }

            return string.Join(", ", changes);
        }
    }

}
