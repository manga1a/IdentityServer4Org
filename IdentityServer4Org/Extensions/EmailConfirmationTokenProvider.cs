using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdentityServer4Org.Extensions
{
    public class EmailConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public EmailConfirmationTokenProvider(
            IDataProtectionProvider dataProtectionProvider, 
            IOptions<EmailConfirmationTokenProviderOptions> options,
            ILogger<EmailConfirmationTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
        {
        }
    }

    public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
    {
    }
}
