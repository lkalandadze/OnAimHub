using System.Security.Cryptography;

namespace RNG;

public static class RNG
{
    public static int Next(int exclusive) { return RandomNumberGenerator.GetInt32(exclusive); }
    public static int NextInMillion() { return RandomNumberGenerator.GetInt32(1_000_000); }
}