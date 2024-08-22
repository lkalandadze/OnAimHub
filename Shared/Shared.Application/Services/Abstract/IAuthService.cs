using Consul;

namespace Shared.Application.Services.Abstract;

public interface IAuthService
{
    int GetCurrentPlayerSegmentId();

    string GetCurrentPlayerUserName();

    int GetCurrentPlayerId();
}