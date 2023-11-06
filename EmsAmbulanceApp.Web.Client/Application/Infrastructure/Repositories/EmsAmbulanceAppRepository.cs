using EmsAmbulanceApp.Web.Client.Application.Data;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmsAmbulanceApp.Web.Client.Application.Infrastructure.Repositories;

public class EmsAmbulanceAppRepository<T> : IEmsAmbulanceAppRepository<T> where T : class
{
    protected readonly EmsAmbulanceAppDbContext _emsAmbulanceAppDbContext;

    public EmsAmbulanceAppRepository(EmsAmbulanceAppDbContext emsAmbulanceAppDbContext)
    {
        _emsAmbulanceAppDbContext = emsAmbulanceAppDbContext;
    }

    public async Task<T?> AddAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        
        var en = await _emsAmbulanceAppDbContext.Set<T>().AddAsync(entity);

        if (en == null)
            return null;

        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<T> listEntity)
    {
        if (!listEntity.Any())
            throw new ArgumentOutOfRangeException(nameof(listEntity));

        await _emsAmbulanceAppDbContext.Set<IEnumerable<T>>().AddRangeAsync(listEntity);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        if (expression == null)
            throw new ArgumentNullException(nameof(expression));
        
        return await _emsAmbulanceAppDbContext.Set<T>().AnyAsync(expression);
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> expression)
    {
        if (expression == null)
            throw new ArgumentNullException(nameof(expression));       

        return await _emsAmbulanceAppDbContext.Set<T>().Where(expression).ToListAsync();
    }

    public async Task<T?> FindAsync(Expression<Func<T, bool>> expression)
    {
        if (expression == null)
            throw new ArgumentNullException(nameof(expression));

        return await _emsAmbulanceAppDbContext.Set<T>().Where(expression).FirstOrDefaultAsync();
    }

    public async Task<T?> FindByIdAsync(int id)
    {
        if (id  <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));

        return await _emsAmbulanceAppDbContext.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _emsAmbulanceAppDbContext.Set<T>().ToListAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await Task.Run(() =>
        {
            _emsAmbulanceAppDbContext.Set<T>().Remove(entity);
        });
    }

    public async Task RemoveRangeAsync(IEnumerable<T> listEntity)
    {
        if (!listEntity.Any())
            throw new ArgumentException(nameof(listEntity));

        await Task.Run(() =>
        {
            _emsAmbulanceAppDbContext.Set<T>().RemoveRange(listEntity);
        });
    }

    public async Task<T> UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await Task.Run(() =>
        {
            _emsAmbulanceAppDbContext.Set<T>().Update(entity);
        });

        return entity;
    }
}
