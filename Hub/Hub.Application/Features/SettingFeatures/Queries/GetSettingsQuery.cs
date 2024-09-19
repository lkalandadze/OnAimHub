using MediatR;
using Shared.Lib.Wrappersl;

namespace Hub.Application.Features.SettingFeatures.Queries;

public class GetSettingsQuery : PagedRequest, IRequest<GetSettingsQueryResponse>;