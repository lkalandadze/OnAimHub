using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract;

public interface ITokenService
{
    string GenerateTokenString(Player player);
}