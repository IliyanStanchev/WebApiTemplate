using MongoDB.Driver;
using WebApiTemplate.Domain.Common;

namespace WebApiTemplate.Infrastructure.Persistence.MongoDb.Extensions
{
    public  static class FilterDefinitionBuilderExtension
    {
        public static FilterDefinition<T> UniqueFilter<T>(this FilterDefinitionBuilder<T> builder, long id) where T : BaseEntity
        {
            FilterDefinition<T> filter = builder.Empty;
            filter &= builder.Eq(x => x.Id, id);
            return filter;
        }
    }
}
