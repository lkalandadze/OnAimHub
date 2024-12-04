using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Delete;

public class DeleteSegmentCommandHandler : ICommandHandler<DeleteSegmentCommand, ApplicationResult>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<DeleteSegmentCommand> _validator;

    public DeleteSegmentCommandHandler(ISegmentService segmentService, IValidator<DeleteSegmentCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(DeleteSegmentCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _segmentService.DeleteSegment(request.Id);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
