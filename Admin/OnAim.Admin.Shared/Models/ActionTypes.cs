namespace OnAim.Admin.Shared.Models;

public static class ActionTypes
{
    public const string Post = "Post";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string Get = "Get";

    public static readonly List<string> All = new List<string> { Post, Update, Delete, Get };
}

public static class EntityNames
{
    public const string User = "Users";
    public const string Endpoint = "Endpoint";
    public const string EndpointGroup = "EndpointGroup";
    public const string Role = "Role";
    public const string Player = "Player";
    public const string Transaction = "Transaction";
    public const string Segment = "Segment";

    public static readonly List<string> All = new List<string> { User, Endpoint, EndpointGroup, Role, Player, Transaction, Segment };
}
