using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.EndPoint.Create
{
    public class CreateEndpointCommandHandler : IRequestHandler<CreateEndpointCommand, ApplicationResult>
    {
        private readonly IValidator<CreateEndpointCommand> _validator;
        private readonly IRepository<Endpoint> _repository;

        public CreateEndpointCommandHandler(
            IValidator<CreateEndpointCommand> validator,
            IRepository<Endpoint> repository
            )
        {
            _validator = validator;
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(CreateEndpointCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existedEndpoint = await _repository.Query(x => x.Name == request.Name).FirstOrDefaultAsync();

            EndpointType endpointTypeEnum = EndpointType.Get;

            if (request.Type != null && Enum.TryParse(request.Type, true, out EndpointType parsedType))
            {
                endpointTypeEnum = parsedType;
            }

            if (existedEndpoint != null)
            {
                throw new AlreadyExistsException("Endpoint with that name already exists.");
            }

            var endpoint = new Endpoint
            {
                Name = request.Name,
                Path = request.Name,
                Description = request.Description ?? "Description needed",
                IsEnabled = true,
                DateCreated = SystemDate.Now,
                IsActive = true,
                Type = endpointTypeEnum,
            };
            await _repository.Store(endpoint);
            await _repository.CommitChanges();

            return new ApplicationResult
            {
                Success = true,
                Data = endpoint.Name,
            };

        }
    }
}
