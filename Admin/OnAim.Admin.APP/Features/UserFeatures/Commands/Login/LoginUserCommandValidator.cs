﻿using FluentValidation;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Model.Email)
            .NotEmpty().EmailAddress()
            .WithMessage("Valid email is required.");
        RuleFor(x => x.Model.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}
