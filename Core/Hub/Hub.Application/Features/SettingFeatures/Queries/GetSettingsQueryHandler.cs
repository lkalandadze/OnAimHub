using Hub.Application.Features.SettingFeatures.Dtos;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Shared.Domain.Abstractions.Repository;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.SettingFeatures.Queries;

public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, GetSettingsQueryResponse>
{
    private readonly IHubSettingRepository _settingRepository;

    public GetSettingsQueryHandler(IHubSettingRepository settingRepository)
    {
        _settingRepository = settingRepository;
    }

    public async Task<GetSettingsQueryResponse> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        var prizes = _settingRepository.Query();

        var total = prizes.Count();

        var prizeList = prizes.Pagination(request).ToList();

        var response = new GetSettingsQueryResponse
        {
            Data = new PagedResponse<SettingsDto>
            (
                prizeList?.Select(x => SettingsDto.MapFrom(x)),
                request.PageNumber,
                request.PageSize,
                total
            ),
        };

        return response;
    }
}