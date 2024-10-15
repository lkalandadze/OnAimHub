using OnAim.Admin.Shared.Helpers.Password;

namespace OnAim.Admin.APP.Services.AuthServices;

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
