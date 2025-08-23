using MangaRead.Application.Repositories;
using MangaRead.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MangaRead.Infrastructure.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;

    public Repository(MangaDbContext context)
    {
        _context = context;
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        try
        {
            _ = await _context.Set<T>().AddAsync(entity);
            return entity;
        }
        catch (DBConcurrencyException ex)
        {
            throw new DuplicateEntityException(ex.Message, ex);
        }


    }

    public virtual Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public virtual async Task<IEnumerable<T>?> GetAllAsync()
    {
        var items = await _context.Set<T>().ToListAsync();
        return items;

    }

    public virtual async Task<IEnumerable<T>?> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        var items = await _context.Set<T>().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return items;
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);

    }



    public virtual void MarkAsModified(T entity)
    {
        if (!_context.Set<T>().Local.Any(e => e == entity))
        {
            _context.Set<T>().Attach(entity);
        }

        _context.Entry(entity).State = EntityState.Modified;


    }
}