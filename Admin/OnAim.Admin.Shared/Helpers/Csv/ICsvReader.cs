namespace OnAim.Admin.Shared.Helpers.Csv;

public interface ICsvReader<T>
{
    IEnumerable<T> Read(Stream stream);
}
