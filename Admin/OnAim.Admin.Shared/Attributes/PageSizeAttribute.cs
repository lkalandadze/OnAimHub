using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.Shared.Attributes
{
    public class PageSizeAttribute : ValidationAttribute
    {
        private readonly int _maxPageSize;

        public PageSizeAttribute(int maxPageSize)
        {
            _maxPageSize = maxPageSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is int pageSize && pageSize > _maxPageSize)
            {
                return new ValidationResult($"PageSize cannot be greater than {_maxPageSize}.");
            }

            return ValidationResult.Success;
        }
    }
}
