﻿namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Update;

public class UpdateEndpointCommandValidator : AbstractValidator<UpdateEndpointCommand>
{
    public UpdateEndpointCommandValidator()
    {
        //RuleFor(x => x.Endpoint.Name)
        //    .NotEmpty()
        //    .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
    }
}
