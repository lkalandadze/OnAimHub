namespace OnAim.Admin.Shared.Excel
{
    public interface IExcelReader<T>
    {
        T Read(Stream stream);
    }
}
