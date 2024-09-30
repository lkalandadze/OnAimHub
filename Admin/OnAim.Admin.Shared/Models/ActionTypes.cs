namespace OnAim.Admin.Shared.Models;

public static class ActionTypes
{
    public const string Create = "CREATE";
    public const string Update = "UPDATE";
    public const string Delete = "DELETE";
    public const string Login = "LOGIN";
    public const string ForgotPassword = "FORGOT_PASSWORD";
    public const string PasswordChange = "PASSWORD_CHANGED";
    public const string Activation = "ACTIVATION";

    public static readonly List<string> All = new List<string> { Create, Update, Delete, Login, ForgotPassword, PasswordChange, Activation };
}

public static class EntityNames
{
    public const string User = "User";
    public const string Endpoint = "Endpoint";
    public const string EndpointGroup = "EndpointGroup";
    public const string Role = "Role";
    public const string Player = "Player";
    public const string Transaction = "Transaction";

    public static readonly List<string> All = new List<string> { User, Endpoint, EndpointGroup, Role, Player, Transaction };
}
