namespace OnAim.Admin.APP.Services.Abstract
{
    public interface IDomainValidationService
    {
        Task<HashSet<string>> GetAllowedDomainsAsync();
        Task<bool> IsDomainAllowedAsync(string email);
    }
}
