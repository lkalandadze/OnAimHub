namespace OnAim.Admin.Shared.Csv
{
    public interface ICsvReader<T>
    {
        IEnumerable<T> Read(Stream stream);
    }
}
