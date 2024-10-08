namespace OnAim.Admin.Shared.DTOs.User;

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