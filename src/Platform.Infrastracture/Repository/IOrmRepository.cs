using Platform.Infrastructure.Core;
using System.Linq.Expressions;

namespace Platform.Infrastructure.Repository.EF
{
    public interface IOrmRepository : IRepository
    {
        Task<T> GetOneAsync<T>(
          Expression<Func<T, bool>>? filter = null,
          string? includeProperties = null)
          where T : class;

        Task<int> GetCountAsync<T>(Expression<Func<T, bool>>? filter = null)
            where T : class;

        Task CommitAsync();
    }
}
