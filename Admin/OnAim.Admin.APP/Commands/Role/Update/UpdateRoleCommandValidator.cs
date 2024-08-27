﻿using FluentValidation;

namespace OnAim.Admin.APP.Commands.Role.Update
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Model.Name)
                .NotEmpty()
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
        }
    }
}
