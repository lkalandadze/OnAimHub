namespace OnAim.Admin.Contracts.Helpers.Csv;

public interface ICsvWriter<T>
{
    void Write(IEnumerable<T> collection, Stream stream);
}
