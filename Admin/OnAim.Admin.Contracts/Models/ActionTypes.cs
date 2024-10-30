namespace OnAim.Admin.Contracts.Models;

public static class ActionTypes
{
    public const string Post = "Post";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string Get = "Get";

    public static readonly List<string> All = new List<string> { Post, Update, Delete, Get };
}
