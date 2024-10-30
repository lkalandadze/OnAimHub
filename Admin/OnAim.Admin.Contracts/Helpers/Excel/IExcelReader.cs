namespace OnAim.Admin.Contracts.Helpers.Excel;

public interface IExcelReader<T>
{
    T Read(Stream stream);
}
