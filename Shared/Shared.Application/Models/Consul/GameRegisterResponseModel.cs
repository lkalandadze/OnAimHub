namespace Shared.Application.Models.Consul;

public class GameRegisterResponseModel : IEquatable<GameRegisterResponseModel>
{
    public int GameVersionId { get; set; }
    public string GameVersionName { get; set; }
    public bool GameVersionIsActive { get; set; }
    public string Address { get; set; }
    public List<int> SegmentIds { get; set; }
    public DateTime ActivationTime { get; set; }
    public List<ConfigurationResponseModel> Configurations { get; set; }

    public bool Equals(GameRegisterResponseModel other)
    {
        if (other == null)
            return false;

        return GameVersionId == other.GameVersionId &&
               GameVersionName == other.GameVersionName &&
               GameVersionIsActive == other.GameVersionIsActive &&
               Address == other.Address &&
               ActivationTime == other.ActivationTime &&
               SegmentIds.SequenceEqual(other.SegmentIds) &&
               Configurations.SequenceEqual(other.Configurations);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as GameRegisterResponseModel);
    }

    public override int GetHashCode()
    {
        // Generate a hash code for the object
        return HashCode.Combine(GameVersionId, GameVersionName, GameVersionIsActive, Address, ActivationTime);
    }
}

public class ConfigurationResponseModel : IEquatable<ConfigurationResponseModel>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }

    public bool Equals(ConfigurationResponseModel other)
    {
        if (other == null)
            return false;

        return Id == other.Id &&
               Name == other.Name &&
               IsDefault == other.IsDefault &&
               IsActive == other.IsActive;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as ConfigurationResponseModel);
    }

    public override int GetHashCode()
    {
        // Generate a hash code for the object
        return HashCode.Combine(Id, Name, IsDefault, IsActive);
    }
}