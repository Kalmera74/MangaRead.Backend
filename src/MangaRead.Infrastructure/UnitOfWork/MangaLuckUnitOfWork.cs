using MangaRead.Application.UnitOfWork;
using MangaRead.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.UnitOfWork;
public class MangaLuckUnitOfWork : IUnitOfWork
{
    private readonly MangaDbContext _dbContext;

    public MangaLuckUnitOfWork(MangaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();

    }
    public void Rollback()
    {
        foreach (var entry in _dbContext.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
        }
    }
}