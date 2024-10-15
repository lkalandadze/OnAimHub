namespace OnAim.Admin.APP.Services.AuthServices;

public interface IPasswordService
{
    string EncryptPassword(string password, string salt);
    string Salt();
}
