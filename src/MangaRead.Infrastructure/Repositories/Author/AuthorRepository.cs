using MangaRead.Application.Repositories.Author;
using MangaRead.Domain.Entities.Author;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.Author;

public class AuthorRepository : Repository<AuthorEntity>, IAuthorRepository
{

    public AuthorRepository(MangaDbContext context) : base(context)
    {

    }

    public async Task<AuthorEntity?> GetByNameAsync(string name)
    {
        return await _context.Set<AuthorEntity>().FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()));
    }

    public async Task<AuthorEntity?> GetBySlugAsync(string slug)
    {
        return await _context.Set<AuthorEntity>().FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<IEnumerable<AuthorEntity>?> SearchByNameAsync(string name)
    {
        return await _context.Set<AuthorEntity>().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
    }
}