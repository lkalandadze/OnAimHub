using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Exceptions;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Delete
{
    public class DeleteEndpointGroupCommandHandler : ICommandHandler<DeleteEndpointGroupCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.EndpointGroup> _repository;
        private readonly IValidator<DeleteEndpointGroupCommand> _validator;

        public DeleteEndpointGroupCommandHandler(
            IRepository<Infrasturcture.Entities.EndpointGroup> repository,
            IValidator<DeleteEndpointGroupCommand> validator
            )
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(DeleteEndpointGroupCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var group = await _repository.Query(x => x.Id == request.GroupId).FirstOrDefaultAsync();

            if (group == null)
            {
                throw new NotFoundException("Permission Group Not Found");
            }

            group.IsDeleted = true;
            group.DateDeleted = SystemDate.Now;

            await _repository.CommitChanges();

            return new ApplicationResult { Success = true };
        }
    }
}
