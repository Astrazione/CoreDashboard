using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CoreDashboard.Extensions
{
    public static class DbSetExtensions
    {
        public static EntityEntry<T>? AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>>? predicate = null) where T : class, new()
        {
            var exists = predicate is not null ? dbSet.Any(predicate) : dbSet.Any();
            return !exists ? dbSet.Add(entity) : null;
        }
    }
}
