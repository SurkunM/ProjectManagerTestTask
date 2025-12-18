namespace ProjectDataManager.Contracts.IUnitOfWork;

public interface IUnitOfWorkTransaction
{
    void BeginTransaction();

    void RollbackTransaction();
}

public interface IUnitOfWork : IUnitOfWorkTransaction, IDisposable
{
    Task SaveAsync();

    T GetRepository<T>() where T : class;

    T GetService<T>() where T : class;
}
