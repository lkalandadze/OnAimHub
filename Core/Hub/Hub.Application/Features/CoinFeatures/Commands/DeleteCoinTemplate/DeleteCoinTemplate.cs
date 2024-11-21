using MediatR;

namespace Hub.Application.Features.CoinFeatures.Commands.DeleteCoinTemplate;

public record DeleteCoinTemplate(int CoinTemplateId) : IRequest;