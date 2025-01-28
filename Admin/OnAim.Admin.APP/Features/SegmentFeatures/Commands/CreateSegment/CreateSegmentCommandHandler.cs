using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Create;

public sealed class CreateSegmentCommandHandler : ICommandHandler<CreateSegmentCommand, ApplicationResult<bool>>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<CreateSegmentCommand> _validator;

    public CreateSegmentCommandHandler(ISegmentService segmentService, IValidator<CreateSegmentCommand> validator) 
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(CreateSegmentCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _segmentService.CreateSegment(request.Id, request.Description, request.PriorityLevel);
    }
}
