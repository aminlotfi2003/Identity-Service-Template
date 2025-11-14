using Asp.Versioning.ApiExplorer;
using IdentityService.Application;
using IdentityService.Infrastructure.CrossCutting;
using IdentityService.Infrastructure.Identity;
using IdentityService.Infrastructure.Persistence;
using IdentityService.WebFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddCrossCutting();
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApiVersioningConfigured();
builder.Services.AddSwaggerConfigured();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.UseMigrationsAndSeedAsync(app.Logger);

app.UseHttpsRedirection();

app.UseAuthorization();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerConfigured(provider);
app.MapControllers();

app.Run();
