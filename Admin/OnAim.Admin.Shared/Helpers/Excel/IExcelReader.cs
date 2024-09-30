namespace OnAim.Admin.Shared.Helpers.Excel;

public interface IExcelReader<T>
{
    T Read(Stream stream);
}
