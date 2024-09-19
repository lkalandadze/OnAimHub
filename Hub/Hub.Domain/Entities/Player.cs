﻿#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Player : BaseEntity<int>
{
    public Player()
    {

    }

    public Player(int id, string userName, List<string> segmentIds = null, int? referrerId = null, ICollection<PlayerBalance>? playerBalances = null)
    {
        Id = id;
        UserName = userName;
        SegmentIds = segmentIds;
        ReferrerId = referrerId;
        PlayerBalances = playerBalances;
    }

    public string UserName { get; private set; }
    public List<string> SegmentIds { get; private set; }
    public ICollection<PlayerBalance>? PlayerBalances { get; set; }
    public int? ReferrerId { get; private set; }
    public bool HasPlayed { get; private set; }

    public const string Base32Chars = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
    private static int ReferralCodeMargin = 10_000_000;

    public void ChangeDetails(string userName, List<string> segmentIds = null, ICollection<PlayerBalance>? playerBalances = null)
    {
        UserName = userName;
        SegmentIds = segmentIds;
        PlayerBalances = playerBalances;
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