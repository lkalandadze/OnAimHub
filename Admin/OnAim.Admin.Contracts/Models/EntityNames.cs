namespace OnAim.Admin.Contracts.Models;

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
