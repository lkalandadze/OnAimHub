#nullable disable

using Shared.Domain.Entities;
using System.Security.Cryptography;

namespace Hub.Domain.Entities;

public class Player : BaseEntity<int>
{
    public Player()
    {

    }

    public Player(int id, string userName, List<string> segmentIds = null, int? recommendedById = null, ICollection<PlayerBalance>? playerBalances = null)
    {
        Id = id;
        UserName = userName;
        SegmentIds = segmentIds;
        RecommendedById = recommendedById;
        PlayerBalances = playerBalances;
        ReferralCode = GenerateUniqueCode();
    }

    public string UserName { get; private set; }
    public List<string> SegmentIds { get; private set; }
    public ICollection<PlayerBalance>? PlayerBalances { get; set; }
    public string ReferralCode { get; private set; }
    public int? RecommendedById { get; private set; }

    public void ChangeDetails(string userName, List<string> segmentIds = null, ICollection<PlayerBalance>? playerBalances = null)
    {
        UserName = userName;
        SegmentIds = segmentIds;
        PlayerBalances = playerBalances;
    }

    public static string GenerateUniqueCode(int length = 5)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] data = new byte[length];
            rng.GetBytes(data);
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                var rnd = data[i] % chars.Length;
                result[i] = chars[rnd];
            }
            return new string(result);
        }
    }

    public void UpdateRecommendedById(int userId)
    {
        RecommendedById = userId;
    }
}