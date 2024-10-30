namespace OnAim.Admin.Contracts.Helpers;

public static class ActivationCodeHelper
{
    public static int ActivationCode()
    {
        var random = new Random();

        var activationCode = random.Next(100000, 999999);

        return activationCode;
    }
}
