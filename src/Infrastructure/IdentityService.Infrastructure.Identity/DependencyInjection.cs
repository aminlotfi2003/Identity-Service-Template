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
            opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            opt.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
        });

        var tokenCfg = identity.GetSection("Tokens").Get<IdentityTokenSettings>() ?? new();
        services.Configure<DataProtectionTokenProviderOptions>(o =>
        {
            o.TokenLifespan = tokenCfg.DefaultLifespan;
        });

        // Cookie authentication (server-side)
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opt =>
            {
                var cookieCfg = identity.GetSection("Cookie").Get<IdentityCookieSettings>() ?? new();

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

        services.Configure<SecurityStampValidatorOptions>(o =>
        {
            o.ValidationInterval = TimeSpan.FromMinutes(5);
        });

        // Custom validators (Password history, min diff, etc.)
        services.AddTransient<IPasswordValidator<ApplicationUser>, PasswordHistoryValidator>();

        return services;
    }
}
