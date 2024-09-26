namespace OnAim.Admin.Shared.Helpers.Excel;

public interface IExcelWriter<T>
{
    void Write(T data, Stream stream);
}
