namespace OnAim.Admin.Shared.Models
{
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
}
