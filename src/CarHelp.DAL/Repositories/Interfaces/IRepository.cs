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
        // TODO: deal with disposing this
        IQueryable<T> GetQueryable();

        Task InsertAsync(T item);
        Task<int> InsertWithIdAsync(T item);
        Task DeleteAsync(T item);
        Task UpdateAsync(T item);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, T>> selector = null, Expression<Func<T, object>> include = null);
        Task<IEnumerable<T>> AllAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, T>> selector = null, int? limit = null, int? offset = null, Expression<Func<T, object>> include = null);
    }
}
