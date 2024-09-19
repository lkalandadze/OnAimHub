namespace OnAim.Admin.Shared.Excel
{
    public interface IExcelWriter<T>
    {
        void Write(T data, Stream stream);
    }
}
