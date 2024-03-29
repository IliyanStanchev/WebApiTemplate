using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApiTemplate.Application.Interfaces;
using WebApiTemplate.Application.Repositories;
using WebApiTemplate.Domain.Entities;

namespace WebApiTemplate.Infrastructure.Persistence.MongoDb.Repositories
{
    public class DummyEntityRepository(IMongoClient mongoClient, 
                                       IClientSessionHandle clientSessionHandle,
                                       IDateTime dateTime,
                                       IOptions<DatabaseOptions> settings,
                                       ILogger<DummyEntityRepository> logger)

        : MongoRepository<DummyEntity>(mongoClient, clientSessionHandle, settings, dateTime, CollectionName),
          IDummyEntityRepository
    {
        private const string CollectionName = "dummyEntity";

        public async Task<DummyEntity?> SearchOneAsync(string? title, CancellationToken token)
        {
            var builder = Builders<DummyEntity>.Filter;
            FilterDefinition<DummyEntity> filter = builder.Empty;

            if (string.IsNullOrWhiteSpace(title))
            {
                logger.LogError("Title is required.");
                return null;
            }

            filter &= builder.Eq(x => x.Title, title);
            IAsyncCursor<DummyEntity?> cursor = await Collection.FindAsync(filter, cancellationToken: token);
            return cursor.FirstOrDefault(token);
        }
    }
}
