using WebApiTemplate.Domain.Common;

namespace WebApiTemplate.Application.Repositories;
public interface IRepository<T> where T : BaseEntity
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);

    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);

    Task<T> DeleteAsync(T entity, CancellationToken cancellationToken);

    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
}
