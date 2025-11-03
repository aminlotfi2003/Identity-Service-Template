using IdentityService.Domain.Identity;
using IdentityService.Infrastructure.Identity.Claims;
using IdentityService.Infrastructure.Identity.Options;
using IdentityService.Infrastructure.Identity.Validators;
using IdentityService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config)
    {
        // Options from appsettings (optional, with safe defaults)
        var identity = config.GetSection("Identity");
        services.Configure<IdentityCookieSettings>(identity.GetSection("Cookie"));
        services.Configure<IdentityTokenSettings>(identity.GetSection("Tokens"));

        // ASP.NET Core Identity (Core + Roles + EF Stores + TokenProviders)
        services
            .AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.SignIn.RequireConfirmedAccount = true;
                opt.User.RequireUniqueEmail = true;

                // Password policy
                opt.Password.RequiredLength = 12;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;

                // Lockout policy
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>();

        // Token lifetimes (email confirmation/reset, etc.)
        services.PostConfigure<IdentityOptions>(opt =>
        {
            // config → override
            var tokenCfg = config.GetSection("Identity:Tokens").Get<IdentityTokenSettings>() ?? new();
            opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            opt.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;

            services.Configure<DataProtectionTokenProviderOptions>(o =>
            {
                o.TokenLifespan = tokenCfg.DefaultLifespan;
            });
        });

        // Security stamp revalidation (re-authn for sensitive ops)
        services.Configure<SecurityStampValidatorOptions>(o =>
        {
            o.ValidationInterval = TimeSpan.FromMinutes(5);
        });

        // Cookie authentication (server-side)
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opt =>
            {
                var provider = services.BuildServiceProvider();
                var cookieCfg = provider.GetRequiredService<IOptions<IdentityCookieSettings>>().Value;

                opt.Cookie.Name = cookieCfg.Name;
                opt.Cookie.HttpOnly = true;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.LoginPath = cookieCfg.LoginPath;
                opt.LogoutPath = cookieCfg.LogoutPath;
                opt.AccessDeniedPath = cookieCfg.AccessDeniedPath;
                opt.SlidingExpiration = false;
                opt.ExpireTimeSpan = cookieCfg.ExpireTimeSpan;
            });

        // Custom validators (Password history, min diff, etc.)
        services.AddTransient<IPasswordValidator<ApplicationUser>, PasswordHistoryValidator>();

        return services;
    }
}
