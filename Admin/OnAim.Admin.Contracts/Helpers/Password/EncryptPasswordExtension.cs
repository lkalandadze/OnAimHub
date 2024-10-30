using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace OnAim.Admin.Contracts.Helpers.Password;

public static class EncryptPasswordExtension
{
    public static string EncryptPassword(string password, string salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                     password: password,
                                                     salt: Convert.FromBase64String(salt),
                                                     prf: KeyDerivationPrf.HMACSHA256,
                                                     iterationCount: 100000,
                                                     numBytesRequested: 256 / 8));
    }

    public static string Salt()
    {
        byte[] salt = new byte[128 / 8];

        RandomNumberGenerator.Fill(salt);

        return Convert.ToBase64String(salt);
    }
}
