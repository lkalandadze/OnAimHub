using Hub.Application.Features.SettingFeatures.Dtos;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.SettingFeatures.Queries;

public class GetSettingsQueryResponse : Response<PagedResponse<SettingsDto>>;
