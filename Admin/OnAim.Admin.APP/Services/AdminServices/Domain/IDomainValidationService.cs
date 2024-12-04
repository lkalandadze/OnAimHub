namespace OnAim.Admin.APP.Services.AdminServices.Domain;

public interface IDomainValidationService
{
    Task<HashSet<string>> GetAllowedDomainsAsync();
    Task<bool> IsDomainAllowedAsync(string email);
}
