namespace OnAim.Admin.Shared.Helpers
{
    public static class PasswordHelper
    {
        private static readonly Random Random = new Random();

        public static string GenerateTemporaryPassword(int length = 12)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray());
        }
    }
}
