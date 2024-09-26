using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.DTOs.User;
using System.Security.Claims;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure.Configuration;

namespace OnAim.Admin.APP.Feature.Identity;

public record RefreshTokenCommand(string RefreshToken) : ICommand<AuthResultDto>;

public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, AuthResultDto>
{
    private readonly IRepository<RefreshToken> _repository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<AccessToken> _accessTokenRepository;
    private readonly IJwtFactory _jwtFactory;

    public RefreshTokenHandler(
        IRepository<RefreshToken> repository,
        IRepository<User> userRepository,
        IRepository<AccessToken> accessTokenRepository,
        IJwtFactory jwtFactory
        )
    {
        _repository = repository;
        _userRepository = userRepository;
        _accessTokenRepository = accessTokenRepository;
        _jwtFactory = jwtFactory;
    }

    public async Task<AuthResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _repository.Query(rt => rt.Token == request.RefreshToken && !rt.IsRevoked)
                                 .FirstOrDefaultAsync();

        if (refreshToken == null || refreshToken.Expiration <= DateTime.UtcNow)
        {
            throw new BadRequestException("Invalid refresh token.");
        }

        var user = await _userRepository.Query(x => x.Id == refreshToken.UserId).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new BadRequestException("User not found.");
        }

        await _jwtFactory.RevokeRefreshToken(request.RefreshToken);

        var oldAccessToken = await _accessTokenRepository.Query(at => at.UserId == user.Id)
                               .FirstOrDefaultAsync();

        if (oldAccessToken != null)
        {
            await _jwtFactory.RevokeAccessToken(oldAccessToken.Token);
        }


        var newAccessToken = _jwtFactory.GenerateEncodedToken(user.Id, user.Email, new List<Claim>(), user.UserRoles.Select(x => x.Role.Name));
        var newRefreshToken = await _jwtFactory.GenerateRefreshToken(user.Id);

        return new AuthResultDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}
