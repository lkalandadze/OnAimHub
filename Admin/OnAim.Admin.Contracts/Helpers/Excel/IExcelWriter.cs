namespace OnAim.Admin.Contracts.Helpers.Excel;

public interface IExcelWriter<T>
{
    void Write(T data, Stream stream);
}
