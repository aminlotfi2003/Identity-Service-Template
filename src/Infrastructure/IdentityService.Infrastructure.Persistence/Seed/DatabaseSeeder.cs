using IdentityService.Application.Abstractions.Persistence.Repositories.Authorization;
using IdentityService.Application.Abstractions.Persistence.Repositories.Classification;
using IdentityService.Application.Abstractions.Persistence;
using IdentityService.Domain.Authorization;
using IdentityService.Domain.Classification;
using IdentityService.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Persistence.Seed;

public sealed class DatabaseSeeder
{
    private readonly IPermissionRepository _permissionRepo;
    private readonly IDataLabelRepository _labelRepo;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly SeedSettings _settings;
    private readonly UserManager<ApplicationUser>? _userManager;
    private readonly RoleManager<ApplicationRole>? _roleManager;

    public DatabaseSeeder(
        IPermissionRepository permissionRepo,
        IDataLabelRepository labelRepo,
        IUnitOfWork uow,
        IOptions<SeedSettings> settings,
        ILogger<DatabaseSeeder> logger,
        IServiceProvider sp)
    {
        _permissionRepo = permissionRepo;
        _labelRepo = labelRepo;
        _uow = uow;
        _logger = logger;
        _settings = settings.Value;
        _userManager = sp.GetService<UserManager<ApplicationUser>>();
        _roleManager = sp.GetService<RoleManager<ApplicationRole>>();
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        if (!_settings.Enable)
        {
            _logger.LogInformation("Seeding disabled.");
            return;
        }

        await SeedPermissionsAsync(ct);
        await SeedDataLabelsAsync(ct);
        await SeedRolesAsync(ct);
        await SeedAdminAsync(ct);
    }

    #region Seed Permissions
    private async Task SeedPermissionsAsync(CancellationToken ct)
    {
        foreach (var name in _settings.DefaultPermissions.Distinct())
        {
            var exists = await _permissionRepo.GetByNameAsync(name, ct);
            if (exists is null)
                await _permissionRepo.AddAsync(new Permission { Id = Guid.NewGuid(), Name = name, Category = name.Split('.')[0] }, ct);
        }
        await _uow.SaveChangesAsync(ct);
    }
    #endregion

    #region Seed DataLabels
    private async Task SeedDataLabelsAsync(CancellationToken ct)
    {
        foreach (var name in _settings.DefaultDataLabels.Distinct())
        {
            var exists = await _labelRepo.GetByNameAsync(name, ct);
            if (exists is null)
                await _labelRepo.AddAsync(new DataLabel { Id = Guid.NewGuid(), Name = name }, ct);
        }
        await _uow.SaveChangesAsync(ct);
    }
    #endregion

    #region Seed Roles
    private async Task SeedRolesAsync(CancellationToken ct)
    {
        if (_roleManager is null)
        {
            _logger.LogWarning("RoleManager not available. Skipping role seed.");
            return;
        }

        foreach (var roleName in _settings.DefaultRoles.Distinct())
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                role = new ApplicationRole { Id = Guid.NewGuid(), Name = roleName, NormalizedName = roleName.ToUpperInvariant() };
                var res = await _roleManager.CreateAsync(role);
                if (!res.Succeeded)
                    _logger.LogWarning("Failed to create role {Role}: {Errors}", roleName, string.Join(", ", res.Errors.Select(e => e.Description)));
            }
        }
    }
    #endregion

    #region Seed Admin
    private async Task SeedAdminAsync(CancellationToken ct)
    {
        if (_userManager is null || _roleManager is null)
        {
            _logger.LogWarning("UserManager/RoleManager not available. Skipping admin seed.");
            return;
        }

        var admin = await _userManager.FindByNameAsync(_settings.AdminUserName);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = _settings.AdminUserName,
                NormalizedUserName = _settings.AdminUserName.ToUpperInvariant(),
                Email = _settings.AdminEmail,
                NormalizedEmail = _settings.AdminEmail.ToUpperInvariant(),
                EmailConfirmed = true,
                MustChangePasswordOnFirstLogin = true,
                IsActive = true
            };
            var create = await _userManager.CreateAsync(admin, _settings.AdminPassword);
            if (!create.Succeeded)
            {
                _logger.LogWarning("Failed to create admin user: {Errors}", string.Join(", ", create.Errors.Select(e => e.Description)));
                return;
            }
        }

        // Role Admin
        var adminRole = await _roleManager.FindByNameAsync(_settings.AdminRoleName);
        if (adminRole is not null && !await _userManager.IsInRoleAsync(admin, _settings.AdminRoleName))
        {
            var r = await _userManager.AddToRoleAsync(admin, _settings.AdminRoleName);
            if (!r.Succeeded)
                _logger.LogWarning("Failed to add admin to role: {Errors}", string.Join(", ", r.Errors.Select(e => e.Description)));
        }
    }
    #endregion
}
