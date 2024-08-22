﻿using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Update
{
    public sealed class UpdateEndpointCommandHandler : IRequestHandler<UpdateEndpointCommand, ApplicationResult>
    {
        private readonly IEndpointRepository _endpointRepository;
        private readonly IValidator<UpdateEndpointCommand> _validator;

        public UpdateEndpointCommandHandler(
            IEndpointRepository endpointRepository,
            IValidator<UpdateEndpointCommand> validator
            )
        {
            _endpointRepository = endpointRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(UpdateEndpointCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new ApplicationResult { Success = false };
            }

            await _endpointRepository.UpdateEndpoint(request.Id, request.Endpoint);

            return new ApplicationResult
            {
                Success = true,
                Data = null,
                Errors = null
            };
        }
    }
}
