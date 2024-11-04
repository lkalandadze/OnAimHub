namespace OnAim.Admin.Contracts.Helpers;

public static class EnumHelper
{
    public static List<EnumValueDto> GetEnumValues<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
                   .Cast<T>()
                   .Select(e => new EnumValueDto
                   {
                       Value = Convert.ToInt32(e),
                       Name = e.ToString()
                   })
                   .ToList();
    }
}
public class EnumValueDto
{
    public int Value { get; set; }
    public string Name { get; set; }
}
