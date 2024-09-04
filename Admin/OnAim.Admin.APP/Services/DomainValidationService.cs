using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Exceptions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Services
{
    public class DomainValidationService : IDomainValidationService
    {
        private readonly IRepository<AllowedDomain> _repository;
        private HashSet<string> _cachedAllowedDomains;
        private readonly object _lock = new object();

        public DomainValidationService(IRepository<AllowedDomain> repository)
        {
            _repository = repository;
        }

        public async Task<HashSet<string>> GetAllowedDomainsAsync()
        {
            var domains = await _repository
                .Query()
                .Select(d => d.Domain)
                .ToListAsync();

            if (domains == null) throw new NotFoundException("Domains Not Found");

            return new HashSet<string>(domains);
        }

        private async Task EnsureAllowedDomainsLoadedAsync()
        {
            if (_cachedAllowedDomains == null)
            {
                lock (_lock)
                {
                    if (_cachedAllowedDomains == null)
                    {
                        _cachedAllowedDomains = GetAllowedDomainsAsync().GetAwaiter().GetResult();
                    }
                }
            }
        }

        public async Task<bool> IsDomainAllowedAsync(string email)
        {
            await EnsureAllowedDomainsLoadedAsync();
            var emailDomain = email.Split('@').Last();
            return _cachedAllowedDomains.Contains(emailDomain);
        }
    }
}
