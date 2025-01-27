using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Feature.Identity;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Activate;
using OnAim.Admin.APP.Feature.UserFeature.Commands.ChangePassword;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Create;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Delete;
using OnAim.Admin.APP.Feature.UserFeature.Commands.ForgotPassword;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Login;
using OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Registration;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Update;
using OnAim.Admin.APP.Feature.UserFeature.Queries.GetAllUser;
using OnAim.Admin.APP.Feature.UserFeature.Queries.GetById;
using OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;
using OnAim.Admin.APP.Features.UserFeatures.Commands.TwoFA;
using OnAim.Admin.APP.Services.Hub.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.ApplicationInfrastructure.Validation;
using OnAim.Admin.Contracts.Dtos.User;
using System.Net;
using System.Security.Claims;

namespace OnAim.Admin.API.Controllers;

public class TestController : ApiControllerBase
{
    private PromotionService _promotionService;
    public TestController(PromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    //public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionDto create)
    //        => Ok(await _promotionService.CreatePromotion(create));

    //public async Task<IActionResult> Test()
    //{
    //    var promotionData = new CreatePromotionDto();

    //    var guid = await _promotionService.CreatePromotion(promotionData);
    //}
}
