using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.SettingFeatures.Queries;

public class GetSettingsQuery : PagedRequest, IRequest<GetSettingsQueryResponse>;