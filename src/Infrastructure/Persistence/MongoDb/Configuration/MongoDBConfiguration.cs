using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace WebApiTemplate.Infrastructure.Persistence.MongoDb.Configuration
{
    public class MongoDbConfiguration
    {
        public MongoDbConfiguration(Assembly assembly)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            ConventionPack pack = new() { new CamelCaseElementNameConvention() };

            ConventionRegistry.Register(nameof(CamelCaseElementNameConvention), pack, _ => true);

            var types = assembly.GetExportedTypes()
                                       .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMongoEntityConfiguration<>)))
                                       .ToList();

            foreach (Type type in types)
            {
                object? instance = Activator.CreateInstance(type);
                MethodInfo? methodInfo = type.GetMethod("Configure");
                if (methodInfo != null)
                {
                    methodInfo?.Invoke(instance, null);
                }
            }
        }
    }
}
