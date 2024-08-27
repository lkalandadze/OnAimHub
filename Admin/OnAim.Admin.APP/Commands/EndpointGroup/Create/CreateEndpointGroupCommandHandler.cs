using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Extensions;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using static OnAim.Admin.APP.Extensions.Extension;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Create
{
    public class CreateEndpointGroupCommandHandler : IRequestHandler<CreateEndpointGroupCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.EndpointGroup> _repository;
        private readonly IRepository<Endpoint> _endpointRepository;
        private readonly IValidator<CreateEndpointGroupCommand> _validator;

        public CreateEndpointGroupCommandHandler(
            IRepository<Infrasturcture.Entities.EndpointGroup> repository,
            IRepository<Endpoint> endpointRepository,
            IValidator<CreateEndpointGroupCommand> validator
            )
        {
            _repository = repository;
            _endpointRepository = endpointRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(CreateEndpointGroupCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existedGroupName = await _repository.Query(x => x.Name == request.Model.Name).FirstOrDefaultAsync();

            if (existedGroupName == null)
            {
                var endpointGroup = new Infrasturcture.Entities.EndpointGroup
                {
                    Name = request.Model.Name,
                    Description = request.Model.Description,
                    IsEnabled = true,
                    IsActive = true,
                    EndpointGroupEndpoints = new List<EndpointGroupEndpoint>(),
                    DateCreated = SystemDate.Now,
                    UserId = HttpContextAccessorProvider.HttpContextAccessor.GetUserId()
                };

                foreach (var endpointId in request.Model.EndpointIds)
                {
                    var endpoint = await _endpointRepository.Query(x => x.Id == endpointId).FirstOrDefaultAsync();

                    if (!endpoint.IsEnabled)
                    {
                        return new ApplicationResult { Success = false, Data = $"Permmission Is Disabled!" };
                    }

                    var endpointGroupEndpoint = new EndpointGroupEndpoint
                    {
                        Endpoint = endpoint,
                        EndpointGroup = endpointGroup
                    };

                    endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
                }

                await _repository.Store(endpointGroup);
                await _repository.CommitChanges();
            }
            else
            {
                return new ApplicationResult { Success = false, Data = $"Permmission Group with that name already exists!" };
            }

            return new ApplicationResult
            {
                Success = true,
                Data = $"Permmission Group {request.Model.Name} Successfully Created",
            };
        }
    }
}
