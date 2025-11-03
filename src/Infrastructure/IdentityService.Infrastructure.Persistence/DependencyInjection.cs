using IdentityService.Application.Abstractions.Persistence.Repositories.Audit;
using IdentityService.Application.Abstractions.Persistence.Repositories.Authorization;
using IdentityService.Application.Abstractions.Persistence.Repositories.Classification;
using IdentityService.Application.Abstractions.Persistence.Repositories.Identity;
using IdentityService.Application.Abstractions.Persistence.Repositories.Security;
using IdentityService.Application.Abstractions.Persistence;
using IdentityService.Infrastructure.Persistence.Repositories.Audit;
using IdentityService.Infrastructure.Persistence.Repositories.Authorization;
using IdentityService.Infrastructure.Persistence.Repositories.Classification;
using IdentityService.Infrastructure.Persistence.Repositories.Identity;
using IdentityService.Infrastructure.Persistence.Repositories.Security;
using IdentityService.Infrastructure.Persistence.Seed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        var conn = config.GetConnectionString("Default")
                   ?? throw new InvalidOperationException("ConnectionStrings:Default is missing.");

        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(conn, sql =>
            {
                sql.MigrationsHistoryTable("__EFMigrationsHistory", ApplicationDbContext.IamSchema);
                sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
        });

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
        services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();

        services.AddScoped<IUserLoginHistoryRepository, UserLoginHistoryRepository>();

        services.AddScoped<IDataLabelRepository, DataLabelRepository>();
        services.AddScoped<ILabeledResourceRepository, LabeledResourceRepository>();

        services.AddScoped<IApplicationUserClaimRepository, ApplicationUserClaimRepository>();
        services.AddScoped<IApplicationRoleClaimRepository, ApplicationRoleClaimRepository>();
        services.AddScoped<IApplicationUserLoginRepository, ApplicationUserLoginRepository>();
        services.AddScoped<IApplicationUserTokenRepository, ApplicationUserTokenRepository>();
        services.AddScoped<IApplicationUserRoleLinkRepository, ApplicationUserRoleLinkRepository>();

        services.AddScoped<IPasswordHistoryRepository, PasswordHistoryRepository>();
        services.AddScoped<ISecurityBannerAcceptanceRepository, SecurityBannerAcceptanceRepository>();

        // Seed
        services.AddScoped<DatabaseSeeder>();
        services.Configure<SeedSettings>(config.GetSection("Seed"));

        return services;
    }

    public static async Task UseMigrationsAndSeedAsync(this IServiceProvider sp, ILogger? logger = null, CancellationToken ct = default)
    {
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync(ct);
        logger?.LogInformation("Database migrated.");

        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedAsync(ct);
        logger?.LogInformation("Database seed completed.");
    }
}
