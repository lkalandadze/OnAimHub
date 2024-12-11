using Hub.Api.Models.Games;
using Hub.Api.Models.Segments;
using Hub.Application.Features.CoinFeatures.Commands.CreateCoinTemplate;
using Hub.Application.Features.CoinFeatures.Commands.DeleteCoinTemplate;
using Hub.Application.Features.CoinFeatures.Commands.UpdateCoinTemplate;
using Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.CreateWithdrawOption;
using Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.CreateWithdrawOptionEndpoint;
using Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.CreateWithdrawOptionGroup;
using Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.UpdateWithdrawOption;
using Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.UpdateWithdrawOptionEndpoint;
using Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.UpdateWithdrawOptionGroup;
using Hub.Application.Features.GameFeatures.Dtos;
using Hub.Application.Features.GameFeatures.Queries.GetAllGame;
using Hub.Application.Features.PlayerBanFeatures.Commands.Create;
using Hub.Application.Features.PlayerBanFeatures.Commands.Revoke;
using Hub.Application.Features.PlayerBanFeatures.Commands.Update;
using Hub.Application.Features.PrizeClaimFeatures.Commands.CreateReward;
using Hub.Application.Features.PrizeClaimFeatures.Commands.DeleteReward;
using Hub.Application.Features.PromotionFeatures.Commands.CreatePromotion;
using Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionView;
using Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionViewTemplate;
using Hub.Application.Features.PromotionFeatures.Commands.Delete;
using Hub.Application.Features.PromotionFeatures.Commands.SoftDelete;
using Hub.Application.Features.PromotionFeatures.Commands.UpdateStatus;
using Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentsToPlayers;
using Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentToPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentForPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentsForPlayers;
using Hub.Application.Features.SegmentFeatures.Commands.CreateSegment;
using Hub.Application.Features.SegmentFeatures.Commands.DeleteSegment;
using Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentsToPlayers;
using Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentToPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentForPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentsForPlayers;
using Hub.Application.Features.SegmentFeatures.Commands.UpdateSegment;
using Hub.Application.Features.SettingFeatures.Commands.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Lib.Wrappers;

namespace Hub.Api.Controllers;

//[Authorize(AuthenticationSchemes = "BasicAuthentication")]
[ApiExplorerSettings(GroupName = "admin")]
public class AdminController : BaseApiController
{
    #region Promotions

    [HttpPost(nameof(CreatePromotion))]
    public async Task<ActionResult<int>> CreatePromotion(CreatePromotionCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost(nameof(UpdatePromotionStatus))]
    public async Task<Unit> UpdatePromotionStatus(UpdatePromotionStatusCommand request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost(nameof(CreatePromotionView))]
    public async Task<ActionResult> CreatePromotionView(CreatePromotionView command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost(nameof(CreatePromotionViewTemplate))]
    public async Task<ActionResult> CreatePromotionViewTemplate(CreatePromotionViewTemplate command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpDelete(nameof(DeletePromotion))]
    public async Task<Unit> DeletePromotion(DeletePromotionCommand command) => await Mediator.Send(command);

    [HttpPut(nameof(SoftDeletePromotion))]
    public async Task<Unit> SoftDeletePromotion(SoftDeletePromotionCommand command) => await Mediator.Send(command);

    #endregion

    #region Games

    [HttpGet(nameof(AllGames))]
    public async Task<ActionResult<Response<IEnumerable<GameBaseDtoModel>>>> AllGames([FromQuery] GameRequestModel model)
    {
        var query = new GetAllGameQuery(false)
        {
            Name = model.Name,
            SegmentIds = model.SegmentIds,
        };

        return Ok(await Mediator.Send(query));
    }

    #endregion

    #region Players


    [HttpPut(nameof(UpdateBannedPlayer))]
    public async Task<ActionResult> UpdateBannedPlayer(UpdatePlayerBanCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    [HttpPut(nameof(RevokePlayerBan))]
    public async Task<ActionResult> RevokePlayerBan(RevokePlayerBanCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    [HttpPost(nameof(BanPlayer))]
    public async Task<ActionResult> BanPlayer(CreatePlayerBanCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    #endregion

    #region Segments

    [HttpPost(nameof(CreateSegment))]
    public async Task<IActionResult> CreateSegment([FromBody] CreateSegmentCommand command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(201, result);
    }

    [HttpPost(nameof(AssignSegmentToPlayer))]
    public async Task<IActionResult> AssignSegmentToPlayer(string segmentId, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new AssignSegmentToPlayerCommand(segmentId, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(UnassignSegmentToPlayer))]
    public async Task<IActionResult> UnassignSegmentToPlayer(string segmentId, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new UnassignSegmentToPlayerCommand(segmentId, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(AssignSegmentsToPlayers))]
    public async Task<IActionResult> AssignSegmentsToPlayers([FromForm] PlayersSegmentRequestModel request)
    {
        var command = new AssignSegmentsToPlayersCommand(request.SegmentIds, request.File, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(UnassignSegmentsToPlayers))]
    public async Task<IActionResult> UnassignSegmentsToPlayers([FromForm] PlayersSegmentRequestModel request)
    {
        var command = new UnassignSegmentsToPlayersCommand(request.SegmentIds, request.File, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(BlockSegmentForPlayer))]
    public async Task<IActionResult> BlockSegmentForPlayer(string segmentId, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new BlockSegmentForPlayerCommand(segmentId, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(UnblockSegmentForPlayer))]
    public async Task<IActionResult> UnblockSegmentForPlayer(string segmentId, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new UnblockSegmentForPlayerCommand(segmentId, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(BlockSegmentsForPlayers))]
    public async Task<IActionResult> BlockSegmentsForPlayers([FromForm] PlayersSegmentRequestModel request)
    {
        var command = new BlockSegmentsForPlayersCommand(request.SegmentIds, request.File, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(UnblockSegmentsForPlayers))]
    public async Task<IActionResult> UnblockSegmentsForPlayers([FromForm] PlayersSegmentRequestModel request)
    {
        var command = new UnblockSegmentsForPlayersCommand(request.SegmentIds, request.File, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPut(nameof(UpdateSegment))]
    public async Task<IActionResult> UpdateSegment([FromBody] UpdateSegmentCommand command)
    {
        _ = await Mediator.Send(command);

        return StatusCode(200);
    }

    [HttpDelete(nameof(DeleteSegment))]
    public async Task<IActionResult> DeleteSegment(string id)
    {
        _ = await Mediator.Send(new DeleteSegmentCommand(id));

        return Ok();
    }

    #endregion

    #region Coins

    [HttpPost(nameof(CreateCoinTemplate))]
    public async Task<IActionResult> CreateCoinTemplate([FromBody] CreateCoinTemplate command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPut(nameof(UpdateCoinTemplate))]
    public async Task<IActionResult> UpdateCoinTemplate([FromBody] UpdateCoinTemplate command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(DeleteCoinTemplate))]
    public async Task<IActionResult> DeleteCoinTemplate([FromBody] DeleteCoinTemplate command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    #endregion

    #region WithdrawOptions

    [HttpPost(nameof(CreateWithdrawOption))]
    public async Task<IActionResult> CreateWithdrawOption([FromBody] CreateWithdrawOption command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(201, result);
    }

    [HttpPut(nameof(UpdateWithdrawOption))]
    public async Task<IActionResult> UpdateWithdrawOption([FromBody] UpdateWithdrawOption command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(CreateWithdrawOptionEndpoint))]
    public async Task<IActionResult> CreateWithdrawOptionEndpoint([FromBody] CreateWithdrawOptionEndpoint command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(201, result);
    }

    [HttpPut(nameof(UpdateWithdrawOptionEndpoint))]
    public async Task<IActionResult> UpdateWithdrawOptionEndpoint([FromBody] UpdateWithdrawOptionEndpoint command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(CreateWithdrawOptionGroup))]
    public async Task<IActionResult> CreateWithdrawOptionGroup([FromBody] CreateWithdrawOptionGroup command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(201, result);
    }

    [HttpPut(nameof(UpdateWithdrawOptionGroup))]
    public async Task<IActionResult> UpdateWithdrawOptionGroup([FromBody] UpdateWithdrawOptionGroup command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    #endregion

    #region Rewards

    [HttpPost(nameof(CreateReward))]
    public async Task<ActionResult> CreateReward(CreateRewardCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    [HttpPost(nameof(DeleteReward))]
    public async Task<ActionResult> DeleteReward(DeleteRewardCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    #endregion

    #region Settings

    //[HttpGet(nameof(GetSettings))]
    //public async Task<ActionResult<GetSettingsQueryResponse>> GetSettings([FromQuery] GetSettingsQuery request) => await Mediator.Send(request);

    [HttpPut(nameof(UpdateSettings))]
    public async Task<Unit> UpdateSettings(UpdateSettingCommand request) => await Mediator.Send(request);

    #endregion

    #region Referral Distributions

    //[HttpGet(nameof(GetReferralDistributions))]
    //public async Task<ActionResult<GetReferralDistributionsQueryResponse>> GetReferralDistributions([FromQuery] GetReferralDistributionsQuery request) => await Mediator.Send(request);

    //[HttpGet(nameof(GetReferralDistributionById))]
    //public async Task<ActionResult<GetReferralDistributionByIdQueryResponse>> GetReferralDistributionById([FromQuery] GetReferralDistributionByIdQuery request) => await Mediator.Send(request);

    #endregion
}