using Microsoft.EntityFrameworkCore;
using ProjectDataManager.Contracts.IRepositories;

namespace ProjectDataManager.DataAccess.Repositories.BaseAbstractions;

public class BaseEfRepository<T> : IRepository<T> where T : class
{
    protected ProjectDataManagerDbContext DbContext;

    protected DbSet<T> DbSet;

    public BaseEfRepository(ProjectDataManagerDbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = DbContext.Set<T>();
    }

    public Task CreateAsync(T entity)
    {
        return DbSet.AddAsync(entity).AsTask();
    }

    public void Delete(T entity)
    {
        if (DbContext.Entry(entity).State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }

        DbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        DbSet.Attach(entity);
        DbContext.Entry(entity).State = EntityState.Modified;
    }
}
