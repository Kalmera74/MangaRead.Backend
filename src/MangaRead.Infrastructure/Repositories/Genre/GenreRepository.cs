using MangaRead.Application.Repositories.Genre;
using MangaRead.Domain.Entities.Genre;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.Genre;
public class GenreRepository : Repository<GenreEntity>, IGenreRepository
{
    public GenreRepository(MangaDbContext context) : base(context)
    {
    }

    public async Task<GenreEntity?> GetByNameAsync(string name)
    {
        return await _context.Set<GenreEntity>().FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<GenreEntity?> GetBySlugAsync(string slug)
    {
        return await _context.Set<GenreEntity>().FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<IList<GenreEntity>?> SearchByNameAsync(string name)
    {
        return await _context.Set<GenreEntity>().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
    }
}