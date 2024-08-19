using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnAim.Admin.Identity.Entities;

namespace OnAim.Admin.Identity.Services
{
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store,
                                      IOptions<IdentityOptions> optionsAccessor,
                                      IPasswordHasher<User> passwordHasher,
                                      IEnumerable<IUserValidator<User>> userValidators,
                                      IEnumerable<IPasswordValidator<User>> passwordValidators,
                                      ILookupNormalizer keyNormalizer,
                                      IdentityErrorDescriber errors,
                                      IServiceProvider services,
                                      ILogger<UserManager<User>> logger) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        { }
    }
}
