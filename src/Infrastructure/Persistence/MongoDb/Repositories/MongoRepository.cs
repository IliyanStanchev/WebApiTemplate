using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApiTemplate.Application.Interfaces;
using WebApiTemplate.Application.Repositories;
using WebApiTemplate.Domain.Common;
using WebApiTemplate.Infrastructure.Persistence.MongoDb.Extensions;

namespace WebApiTemplate.Infrastructure.Persistence.MongoDb.Repositories
{

    public class MongoRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly string _collection;
        private readonly string _databaseName;
        protected readonly IMongoClient _mongoClient;
        protected readonly IClientSessionHandle _clientSessionHandle;
        protected readonly IDateTime _dateTime;

        protected virtual IMongoCollection<T> Collection => _mongoClient.GetDatabase(_databaseName).GetCollection<T>(_collection);

        public MongoRepository(IMongoClient mongoClient,
                               IClientSessionHandle clientSessionHandle,
                               IOptions<DatabaseOptions> settings,
                               IDateTime dateTime,
                               string collection )
        {
            _mongoClient = mongoClient;
            _clientSessionHandle = clientSessionHandle;
            _collection = collection;
            _dateTime = dateTime;
            _databaseName = settings.Value.Database ?? throw new InvalidOperationException(DatabaseOptions.SectionName);

#if DEBUG
            if (!_mongoClient.GetDatabase(_databaseName).ListCollectionNames().ToList().Contains(collection))
            {
                throw new ArgumentException($"Collection {collection} not found in database {_databaseName}");
            }
#endif
        }

        public async Task InsertManyAsync(List<T> entities, CancellationToken token)
        {
            if (entities != null && entities.Count > 0)
            {
                if (entities.First() is BaseAuditableEntity)
                {
                    entities.ForEach(x => ((x as BaseAuditableEntity)!).Created = _dateTime.Now);
                }

                await Collection.InsertManyAsync(_clientSessionHandle, entities, cancellationToken: token)
                            .ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        public async Task ReplaceOneAsync(T entity, CancellationToken token)
        {
            FilterDefinitionBuilder<T> builder = Builders<T>.Filter;
            ReplaceOneResult result = await Collection.ReplaceOneAsync(_clientSessionHandle, builder.UniqueFilter(entity.Id), entity, cancellationToken: token);
            if (!result.IsAcknowledged)
            {
                throw new InvalidOperationException($"Failed to replace entity with id {entity.Id}");
            }

        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await Collection.InsertOneAsync(_clientSessionHandle, entity, cancellationToken: cancellationToken);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Expression<Func<T, string>> func = f => f.Id.ToString("D");
            string value = (string)entity.GetType().GetProperty(func.Body.ToString().Split(".")[1])?.GetValue(entity, null)!;
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(func, value);

            if (entity is BaseAuditableEntity auditableEntity)
            {
                auditableEntity.LastModified = _dateTime.Now;
            }

            await Collection.ReplaceOneAsync(_clientSessionHandle, filter, entity, cancellationToken: cancellationToken);
            return entity;
        }

        public Task<T> DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var builder = Builders<T>.Filter;
            var filter = builder.UniqueFilter(id);
            var cursor = await Collection.FindAsync(filter, cancellationToken: cancellationToken)
                                         .ConfigureAwait(continueOnCapturedContext: false);
            
            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }

        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
