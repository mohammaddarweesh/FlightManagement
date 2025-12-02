using FlightManagement.Domain.Common;

namespace FlightManagement.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets a repository for the specified entity type
    /// </summary>
    IGenericRepository<T> Repository<T>() where T : BaseEntity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

