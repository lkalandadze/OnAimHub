namespace OnAim.Admin.Shared.Csv
{
    public interface ICsvWriter<T>
    {
        void Write(IEnumerable<T> collection, Stream stream);
    }
}
