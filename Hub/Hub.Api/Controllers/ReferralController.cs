using Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Queries.Get;
using Hub.Application.Features.ReferralFeatures.ReferralDistributionFeatures.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers
{
    [AllowAnonymous]
    public class ReferralController : BaseApiController
    {
        #region Referral Distributions

        [HttpGet(nameof(GetReferralDistributions))]
        public async Task<ActionResult<GetReferralDistributionsQueryResponse>> GetReferralDistributions([FromQuery] GetReferralDistributionsQuery request) => await Mediator.Send(request);

        [HttpGet(nameof(GetReferralDistributionById))]
        public async Task<ActionResult<GetReferralDistributionByIdQueryResponse>> GetReferralDistributionById([FromQuery] GetReferralDistributionByIdQuery request) => await Mediator.Send(request);

        #endregion
    }
}
