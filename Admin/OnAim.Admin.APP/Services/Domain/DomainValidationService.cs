using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.SettingServices;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Services.Domain;

public class DomainValidationService : IDomainValidationService
{
    private readonly IRepository<AllowedEmailDomain> _repository;
    private readonly AppSettings _appSettings;
    private HashSet<string> _cachedAllowedDomains;
    private readonly object _lock = new object();

    public DomainValidationService(IRepository<AllowedEmailDomain> repository, AppSettings appSettings)
    {
        _repository = repository;
        _appSettings = appSettings;
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

    private async Task<bool> AreDomainRestrictionsEnabledAsync()
    {
        var settingValue = nameof(_appSettings.DomainRestrictionsEnabled);
        return settingValue != "false";
    }

    public async Task<bool> IsDomainAllowedAsync(string email)
    {
        if (!await AreDomainRestrictionsEnabledAsync())
            return true;

        await EnsureAllowedDomainsLoadedAsync();
        var emailDomain = email.Split('@').Last();
        return _cachedAllowedDomains.Contains(emailDomain);
    }
}
