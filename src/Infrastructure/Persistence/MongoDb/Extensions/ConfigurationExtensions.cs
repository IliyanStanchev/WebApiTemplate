using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using WebApiTemplate.Infrastructure.Persistence.MongoDb.Configuration;

namespace WebApiTemplate.Infrastructure.Persistence.MongoDb.Extensions
{
    public static class ConfigurationExtensions
    {
       public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(_=> new MongoDbConfiguration(Assembly.GetExecutingAssembly()));
            services.AddSingleton<IMongoClient>(c =>
            {
                string? connectionString = configuration.GetValue<string>($"{DatabaseOptions.SectionName}:ConnectionString");
                return new MongoClient(connectionString);
            });
            services.AddScoped(c =>
                c.GetService<IMongoClient>()!.StartSession());
        }
    }
}
