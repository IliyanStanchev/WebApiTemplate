using WebApiTemplate.Domain.Common;

namespace WebApiTemplate.Infrastructure.Persistence.MongoDb.Configuration
{
    public interface IMongoEntityConfiguration<T> where T : BaseEntity
    {
        void Configure();
    }
}
