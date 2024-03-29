using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiTemplate.Application.Interfaces;
using WebApiTemplate.Application.Repositories;
using WebApiTemplate.Domain.Constants;
using WebApiTemplate.Infrastructure.Authorization;
using WebApiTemplate.Infrastructure.Persistence.MongoDb;
using WebApiTemplate.Infrastructure.Persistence.MongoDb.Extensions;
using WebApiTemplate.Infrastructure.Persistence.MongoDb.Repositories;
using WebApiTemplate.Infrastructure.Services;

namespace WebApiTemplate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAuthorizationProvider, JwtAuthorizationProvider>();

        services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services.AddMongoDb(configuration);

        services.AddSingleton(TimeProvider.System);
        
        services.AddScoped<IDateTime, UtcDateTimeService>();

        services.AddAuthorization(options => options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        services.AddBusinessLogic();
        return services;
    }

    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddTransient<IDummyEntityRepository, DummyEntityRepository>();
        
        services.RegisterOptions();

        return services;
    }

    private static void RegisterOptions(this IServiceCollection services)
    {
        services.AddOptions<DatabaseOptions>()
                .BindConfiguration(DatabaseOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();
    }
}
