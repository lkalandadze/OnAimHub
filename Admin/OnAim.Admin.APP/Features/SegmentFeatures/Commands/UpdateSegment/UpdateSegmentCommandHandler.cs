using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Update;

public class UpdateSegmentCommandHandler : ICommandHandler<UpdateSegmentCommand, ApplicationResult>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<UpdateSegmentCommand> _validator;

    public UpdateSegmentCommandHandler(ISegmentService segmentService, IValidator<UpdateSegmentCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(UpdateSegmentCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _segmentService.UpdateSegment(request.Id, request.Description, request.PriorityLevel);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
