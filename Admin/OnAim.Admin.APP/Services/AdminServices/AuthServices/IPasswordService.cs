namespace OnAim.Admin.APP.Services.Admin.AuthServices;

public interface IPasswordService
{
    string EncryptPassword(string password, string salt);
    string Salt();
}
