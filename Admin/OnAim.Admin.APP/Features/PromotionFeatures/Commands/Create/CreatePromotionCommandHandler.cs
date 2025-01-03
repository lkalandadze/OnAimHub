﻿using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Commands.Create;

public class CreatePromotionCommandHandler : ICommandHandler<CreatePromotionCommand, ApplicationResult>
{
    private readonly IPromotionService _promotionService;

    public CreatePromotionCommandHandler(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }
    public async Task<ApplicationResult> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        var result = await _promotionService.CreatePromotion(request.Create);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
