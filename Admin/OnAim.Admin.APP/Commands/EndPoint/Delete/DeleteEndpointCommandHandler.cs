using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.EndPoint.Delete
{
    public class DeleteEndpointCommandHandler : IRequestHandler<DeleteEndpointCommand, ApplicationResult>
    {
        private readonly IRepository<Endpoint> _repository;
        private readonly IValidator<DeleteEndpointCommand> _validator;

        public DeleteEndpointCommandHandler(
            IRepository<Endpoint> repository,
            IValidator<DeleteEndpointCommand> validator
            )
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(DeleteEndpointCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var endpoint = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (endpoint != null)
            {
                endpoint.IsActive = false;
                endpoint.IsEnabled = false;
                endpoint.DateDeleted = SystemDate.Now;
                await _repository.CommitChanges();
            }

            return new ApplicationResult { Success = true };
        }
    }
}
