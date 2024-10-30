namespace OnAim.Admin.Contracts.Helpers.Csv;

public interface ICsvReader<T>
{
    IEnumerable<T> Read(Stream stream);
}
