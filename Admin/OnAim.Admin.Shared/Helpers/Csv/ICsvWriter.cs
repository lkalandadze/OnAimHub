namespace OnAim.Admin.Shared.Helpers.Csv;

public interface ICsvWriter<T>
{
    void Write(IEnumerable<T> collection, Stream stream);
}
