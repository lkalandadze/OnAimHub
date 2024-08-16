#nullable disable

using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Security.Cryptography;

namespace Hub.Application.Configurations;

public class JwtConfig
{
    private string _secretKey;

    public string ValidIssuer { get; set; }

    public string ValidAudience { get; set; }

    public List<string> ValidAudiences { get; set; }

    public string SecretKey
    {
        get
        {
            return _secretKey;
        }
        set
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
                _secretKey =  BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }

    public int ExpiresInMinutes { get; set; }
}