using System.Linq.Expressions;

namespace EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;

public interface IEmsAmbulanceAppRepository<T> where T : class 
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> FindByIdAsync(int id); 
    Task<T?> FindAsync(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> expression);

    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Task AddRangeAsync(IEnumerable<T> listEntity);

    Task<T?> AddAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task RemoveAsync(T entity);
    Task RemoveRangeAsync(IEnumerable<T> listEntity);
}
