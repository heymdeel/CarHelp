using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.DAL.Repositories
{
    public class L2DBRepository<T> : IRepository<T> where T : class
    {
        public IQueryable<T> GetQueryable()
        {
            var db = new L2DBContext();

            return db.GetTable<T>().AsQueryable();
        }

        public async Task CreateAsync(T item)
        {
            using (var db = new L2DBContext())
            {
                await db.InsertAsync(item);
            }
        }

        public async Task RemoveAsync(T item)
        {
            using (var db = new L2DBContext())
            {
                await db.DeleteAsync(item);
            }
        }

        public async Task UpdateAsync(T item)
        {
            using (var db = new L2DBContext())
            {
                await db.UpdateAsync(item);
            }
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, T>> selector = null, int? page = null, int? size = null, Expression<Func<T, object>> include = null)
        {
            using (var db = new L2DBContext())
            {
                IQueryable<T> query;

                if (include != null)
                {
                    query = db.GetTable<T>().LoadWith(include);
                }
                else
                {
                    query = db.GetTable<T>();
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (selector != null)
                {
                    query = query.Select(selector);
                }

                if (page != null && size != null)
                {
                    query = query.Take((page.Value - 1) * size.Value).Skip(size.Value);
                }

                return await query.ToListAsync();
            }
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, T>> selector = null, Expression<Func<T, object>> include = null)
        {
            using (var db = new L2DBContext())
            {
                IQueryable<T> query;

                if (include != null)
                {
                    query = db.GetTable<T>().LoadWith(include);
                }
                else
                {
                    query = db.GetTable<T>();
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (selector != null)
                {
                    query = query.Select(selector);
                }

                return await query.FirstOrDefaultAsync();
            }
        }
    }
}
