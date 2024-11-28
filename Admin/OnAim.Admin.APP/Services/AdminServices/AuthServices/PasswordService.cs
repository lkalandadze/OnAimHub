using OnAim.Admin.Contracts.Helpers.Password;

namespace OnAim.Admin.APP.Services.Admin.AuthServices;

public class PasswordService : IPasswordService
{
    public string EncryptPassword(string password, string salt)
    {
        return EncryptPasswordExtension.EncryptPassword(password, salt);
    }

    public string Salt()
    {
        return EncryptPasswordExtension.Salt();
    }
}
