using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IService<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        //queries does not go to database directly for more performance. It only goes when you call .ToListAsync()
        // productRepository.where(x=>x.id>5).OrderBy.ToListAsync()
        IQueryable<T> Where(Expression<Func<T, bool>> expression);// it gets the entity x and returns the result of the operation as boolean x.id>5
        Task<T> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);// it only changes the entities state in efcore so it does not have async 
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);

    }
}
