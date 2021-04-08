using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UManager.IdentityFilter
{
    public class CustomUserManager<TUser> : UserManager<TUser>, IDisposable where TUser : class
    {
        public CustomUserManager(IUserStore<TUser> store,
                    IOptions<IdentityOptions> optionsAccessor,
                    IPasswordHasher<TUser> passwordHasher,
                    IEnumerable<IUserValidator<TUser>> userValidators,
                    IEnumerable<IPasswordValidator<TUser>> passwordValidators,
                    ILookupNormalizer keyNormalizer,
                    IdentityErrorDescriber errors,
                    IServiceProvider services,
                    ILogger<UserManager<TUser>> logger)
                    : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
