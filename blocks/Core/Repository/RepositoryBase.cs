using Core.Domain;

namespace Core.Repository;

public abstract class RepositoryBase<T> where T : IAggregateRoot
{
    public abstract Task AddAsync(T domainObject);
}