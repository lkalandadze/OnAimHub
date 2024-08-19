using FluentValidation;
using FluentValidation.Results;
using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Behaviors
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request,
                                            RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v =>
                        v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Any())
                {
                    throw new ValidationException(failures);
                }
            }
            return await next();
        }
    }

    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new List<Error>();
        }

        public ValidationException(string message)
            : base(message)
        {
            Errors = new List<Error>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures.Select(x => new Error
            {
                Message = x.ErrorMessage
            }).ToList();
            //Errors = new ReadOnlyDictionary<string, object?>(failures
            //    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            //    .ToDictionary(failureGroup => failureGroup.Key, failureGroup => (object?)failureGroup.ToArray()));
        }

        public IReadOnlyList<Error> Errors { get; }
    }
}
