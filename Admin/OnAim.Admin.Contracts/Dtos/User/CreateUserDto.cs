namespace OnAim.Admin.Contracts.Dtos.User;

public record CreateUserDto(
    string FirstName,
        string LastName,
        string Email,
        string Hashed,
        string Salt,
        string Phone,
        bool IsVerified,
        bool IsActive,
        bool IsSuperAdmin);
