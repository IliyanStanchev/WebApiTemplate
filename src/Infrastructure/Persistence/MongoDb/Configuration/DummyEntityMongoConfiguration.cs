using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using WebApiTemplate.Domain.Entities;

namespace WebApiTemplate.Infrastructure.Persistence.MongoDb.Configuration
{
    public class DummyEntityMongoConfiguration : IMongoEntityConfiguration<DummyEntity>
    {
        public void Configure()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(DummyEntity)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<DummyEntity>(builder =>
            {
                builder.AutoMap();

                builder.MapIdMember(x => x.Id)
                       .SetElementName("id")
                       .SetSerializer(new Int64Serializer());
                builder.SetIgnoreExtraElements(true);
            });
        }
    }
}
