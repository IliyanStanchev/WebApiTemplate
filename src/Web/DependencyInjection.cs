using System.Reflection;
using System.Text;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using WebApiTemplate.Domain.Options;
using WebApiTemplate.Web.Adapters;

namespace WebApiTemplate.Web;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddWebServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddRazorPages();

        builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   var issuerTag = $"{AuthenticationOptions.SectionName}:Issuer";
                   var keyTag = $"{AuthenticationOptions.SectionName}:Key";

                   var issuer = builder.Configuration[issuerTag] ?? throw new InvalidOperationException(issuerTag);
                   var key = builder.Configuration[keyTag] ?? throw new InvalidOperationException(keyTag);

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = issuer,
                       ValidAudience = issuer,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                   };
               });

        builder.Services.AddOpenApiDocument((configure, sp) =>
        {
            configure.Title = "WebApiTemplate API";

            // Add JWT
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.Http,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the text box: Bearer {your JWT token}.",
                Scheme = "bearer",
                BearerFormat = "JWT"
                
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        var serviceName = Assembly.GetEntryAssembly()?.GetName().Name;
        var serviceVersion = Assembly.GetEntryAssembly()?.GetName().Version;

        if (serviceName == null || serviceVersion == null)
        {
            throw new InvalidOperationException("Cannot load assembly name or version");
        }

        SerilogAdapter.CreateLogger(builder.Configuration, builder.Services, serviceName, serviceVersion.ToString());

        RegisterOptions(builder.Services);

        return builder;
    }

    public static IServiceCollection AddKeyVaultIfConfigured(this IServiceCollection services, ConfigurationManager configuration)
    {
        var keyVaultUri = configuration["KeyVaultUri"];
        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            configuration.AddAzureKeyVault(
                new Uri(keyVaultUri),
                new DefaultAzureCredential());
        }

        return services;
    }

    private static void RegisterOptions(IServiceCollection services)
    {
        services.AddOptions<AuthenticationOptions>()
                .BindConfiguration(AuthenticationOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();
    }
}
