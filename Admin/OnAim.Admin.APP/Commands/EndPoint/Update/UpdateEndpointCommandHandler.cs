using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.EndPoint.Update
{
    public sealed class UpdateEndpointCommandHandler : IRequestHandler<UpdateEndpointCommand, ApplicationResult>
    {
        private readonly IRepository<Endpoint> _repository;
        private readonly IValidator<UpdateEndpointCommand> _validator;

        public UpdateEndpointCommandHandler(
            IRepository<Endpoint> repository,
            IValidator<UpdateEndpointCommand> validator
            )
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(UpdateEndpointCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new ApplicationResult { Success = false };
            }

            var ep = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (ep != null)
            {
                ep.Name = request.Endpoint.Name;
                ep.Description = request.Endpoint.Description;
                ep.IsActive = request.Endpoint.IsActive ?? true;
                ep.IsEnabled = request.Endpoint.IsEnabled ?? true;
                ep.DateUpdated = SystemDate.Now;

                await _repository.CommitChanges();
            }

            return new ApplicationResult
            {
                Success = true,
            };
        }
    }
}
