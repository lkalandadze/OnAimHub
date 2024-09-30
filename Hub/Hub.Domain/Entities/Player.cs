#nullable disable

using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
public class Player : BaseEntity<int>
{
    public Player()
    {

    }

    public Player(int id)
    {
        Id = id;
        RegistredOn = null;
        LastVisitedOn = null;
    }

    public Player(int id, string userName, int? referrerId = null, IEnumerable<PlayerSegment> playerSegments = null, IEnumerable<PlayerBalance> playerBalances = null)
    {
        Id = id;
        UserName = userName;
        ReferrerId = referrerId;
        RegistredOn = DateTimeOffset.UtcNow;
        LastVisitedOn = DateTimeOffset.UtcNow;
        PlayerSegments = playerSegments?.ToList() ?? [];
        PlayerBalances = playerBalances?.ToList() ?? [];
    }

    public const string Base32Chars = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
    private static int ReferralCodeMargin = 10_000_000;
    
    public string UserName { get; private set; }
    public int? ReferrerId { get; private set; }
    public bool HasPlayed { get; private set; }
    public DateTimeOffset? RegistredOn { get; private set; }
    public DateTimeOffset? LastVisitedOn { get; private set; }

    public ICollection<PlayerBalance> PlayerBalances { get; private set; }
    public ICollection<PlayerSegment> PlayerSegments { get; private set; }
    public ICollection<PlayerBlockedSegment> PlayerBlockedSegments { get; private set; } 

    public void ChangeDetails(string userName, List<string> segmentIds = null)
    {
        UserName = userName;
    }

    public void SetRegistrationDate()
    {
        RegistredOn = DateTimeOffset.UtcNow;
    }

    public void SetLastVisitDate()
    {
        LastVisitedOn = DateTimeOffset.UtcNow;
    }

    public void UpdateHasPlayed()
    {
        HasPlayed = true;
    }

    //public static string GenerateUniqueCode(int length = 5)
    //{
    //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    //    using (var rng = new RNGCryptoServiceProvider())
    //    {
    //        byte[] data = new byte[length];
    //        rng.GetBytes(data);
    //        char[] result = new char[length];
    //        for (int i = 0; i < length; i++)
    //        {
    //            var rnd = data[i] % chars.Length;
    //            result[i] = chars[rnd];
    //        }
    //        return new string(result);
    //    }
    //}

    public static int PromoToId(string base32Number)
    {
        var result = 0;

        var reversedBase32Number = ReverseString(base32Number);

        for (var i = 0; i < reversedBase32Number.Length; i++)
        {
            var c = reversedBase32Number[i];
            var value = Base32Chars.IndexOf(c);
            result += value * (int)Math.Pow(33, i);
        }

        return result - ReferralCodeMargin;
    }

    private static string ReverseString(string input)
    {
        var charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static string IdToPromo(int n)
    {
        n += ReferralCodeMargin;
        var result = "";

        while (n > 0)
        {
            var remainder = n % 33;
            result = Base32Chars[(int)remainder] + result;
            n /= 33;
        }

        return result;
    }

    public void UpdateRecommendedById(int userId)
    {
        ReferrerId = userId;
    }
}