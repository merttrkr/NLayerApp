using System.Linq.Expressions;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);

        //queries does not go to database directly for more performance. It only goes when you call .ToListAsync()
        // productRepository.where(x=>x.id>5).OrderBy.ToListAsync()
        IQueryable<T> Where(Expression<Func<T,bool>> expression);// it gets the entity x and returns the result of the operation as boolean x.id>5
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);
        Task<T> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);// it only changes the entities state in efcore so it does not have async 
        void Remove(T entity);
        void RemoveRange (IEnumerable<T> entities);

    }
}
