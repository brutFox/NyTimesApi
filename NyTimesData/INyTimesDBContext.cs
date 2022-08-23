using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NyTimesData
{
    public interface INyTimesDBContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
