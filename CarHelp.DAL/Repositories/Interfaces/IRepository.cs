using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetQueryable();

        void TestGenericMethod();

        Task CreateAsync(T item);
        Task RemoveAsync(T item);
        Task UpdateAsync(T item);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, T>> selector = null, Expression<Func<T, object>> include = null);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, T>> selector = null, int? page = null, int? size = null, Expression<Func<T, object>> include = null);
    }
}
