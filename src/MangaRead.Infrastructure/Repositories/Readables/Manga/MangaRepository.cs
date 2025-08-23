using MangaRead.Application.Repositories.Readables.Manga;
using MangaRead.Domain.Entities.Readables.Manga;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace MangaRead.Infrastructure.Repositories.Readables.Manga;
public class MangaRepository : Repository<MangaEntity>, IMangaRepository
{
    public MangaRepository(MangaDbContext context) : base(context)
    {
    }

    public override async Task<MangaEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Set<MangaEntity>()
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .Include(x => x.Chapters)
                            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<IEnumerable<MangaEntity>?> GetAllAsync()
    {
        return await _context.Set<MangaEntity>()
                            .Where(m => m.IsPublished)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }

    public async Task<IEnumerable<MangaEntity>?> GetByAuthorAsync(Guid authorId)
    {
        return await _context.Set<MangaEntity>().Where(x => x.Authors.Any(a => a.Id == authorId))
                            .Where(m => m.IsPublished)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type).ToListAsync();
    }

    public async Task<IEnumerable<MangaEntity>?> GetByAuthorsAsync(Guid[] authorIds)
    {
        return await _context.Set<MangaEntity>().Where(x => x.Authors.Any(a => authorIds.Contains(a.Id)))
                            .Where(m => m.IsPublished)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type).ToListAsync();
    }

    public async Task<IEnumerable<MangaEntity>?> GetByCreatedAtAsync(DateTime startDate, DateTime endDate, int take = 10)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date must be earlier than end date.");
        }

        return await _context.Set<MangaEntity>()
                            .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.IsPublished)
                            .Take(take)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }

    public async Task<IEnumerable<MangaEntity>?> GetByUpdatedAtAsync(DateTime startDate, DateTime endDate, int take = 10)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date must be earlier than end date.");
        }

        return await _context.Set<MangaEntity>()
                            .Where(x => x.UpdatedAt >= startDate && x.UpdatedAt <= endDate && x.IsPublished)
                            .OrderByDescending(x => x.UpdatedAt)
                            .Take(take)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }


    public override async Task<IEnumerable<MangaEntity>?> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.Set<MangaEntity>()
                            .Where(m => m.IsPublished)
                            .Skip((pageNumber - 1) * pageSize).Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .Include(x => x.Chapters).Take(pageSize).ToListAsync();
    }




    public async Task<IEnumerable<MangaEntity>?> GetByGenreAsync(Guid genreId)
    {
        return await _context.Set<MangaEntity>().Where(x => x.Genres.Any(g => g.Id == genreId) && x.IsPublished)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }

    public async Task<IEnumerable<MangaEntity>?> GetByGenresAsync(Guid[] genreIds)
    {
        return await _context.Set<MangaEntity>().Where(x => x.Genres.Any(g => genreIds.Contains(g.Id)) && x.IsPublished).
        Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }

    public async Task<MangaEntity?> GetByRatingAsync(Guid ratingId)
    {
        return await _context.Set<MangaEntity>().Where(x => x.Ratings.Any(r => r.Id == ratingId) && x.IsPublished)
        .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<MangaEntity>?> GetByRatingsAsync(int min, int max)
    {
        return await _context.Set<MangaEntity>().Where(x => x.Rating >= min && x.Rating <= max && x.IsPublished)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }
    public async Task<IEnumerable<MangaEntity>?> GetByRandomAsync(int take = 10)
    {
        return await _context.Set<MangaEntity>()
                            .Where(m => m.IsPublished)
                            .OrderByDescending(x => x.Rating).Take(take)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }

    public async Task<IEnumerable<MangaEntity>?> GetByStatusAsync(Guid statusId)
    {
        return await _context.Set<MangaEntity>()
                            .Where(x => x.Status.Id == statusId && x.IsPublished)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }

    public async Task<IEnumerable<MangaEntity>?> GetByStatusesAsync(Guid[] statusIds)
    {
        return await _context.Set<MangaEntity>()
                            .Where(x => statusIds.Contains(x.Status.Id) && x.IsPublished)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }

    public async Task<MangaEntity?> GetByTitleAsync(string title)
    {

        return await _context.Set<MangaEntity>()
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .FirstOrDefaultAsync(x => x.Title.ToLower() == title.ToLower());
    }

    public async Task<IEnumerable<MangaEntity>?> GetByTypeAsync(Guid typeId)
    {
        return await _context.Set<MangaEntity>()
                            .Where(x => x.Type.Id == typeId && x.IsPublished)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }


    public async Task<IEnumerable<MangaEntity>?> GetByTypesAsync(Guid[] typeIds)
    {
        return await _context.Set<MangaEntity>()
                            .Where(x => typeIds.Contains(x.Type.Id) && x.IsPublished)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }




    public async Task<IEnumerable<MangaEntity>?> GetByViewsAsync(int take = 10)
    {
        return await _context.Set<MangaEntity>()
                            .Where(m => m.IsPublished)
                            .OrderByDescending(x => x.ViewCount).Take(take)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .ToListAsync();
    }

    public async Task<MangaEntity?> GetByWebNovelAsync(Guid webNovelId)
    {
        return await _context.Set<MangaEntity>().Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .FirstOrDefaultAsync(x => x.WebNovel != null && x.WebNovel.Id == webNovelId);
    }

    public async Task<IEnumerable<MangaEntity>?> GetFilteredMangasAsync(Guid[]? typeIds, Guid[]? genreIds, Guid[]? statusIds)
    {

        if (typeIds == null && genreIds == null && statusIds == null)
        {
            return null;
        }

        var query = _context.Set<MangaEntity>().AsQueryable();

        if (typeIds != null && typeIds.Length > 0)
        {
            query = query.Where(m => typeIds.Contains(m.Type.Id) && m.IsPublished);
        }

        if (genreIds != null && genreIds.Length > 0)
        {
            query = query.Where(m => m.Genres.Any(g => genreIds.Contains(g.Id)) && m.IsPublished);
        }

        if (statusIds != null && statusIds.Length > 0)
        {
            query = query.Where(m => statusIds.Contains(m.Status.Id) && m.IsPublished);
        }

        return await query.Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type).ToListAsync();
    }

    public async Task<IEnumerable<MangaEntity>?> GetFilteredMangasPagedAsync(Guid[]? typeIds, Guid[]? genreIds, Guid[]? statusIds, int skip, int take)
    {
        if (typeIds == null && genreIds == null && statusIds == null)
        {
            return null;
        }

        var query = _context.Set<MangaEntity>().AsQueryable();


        if (typeIds != null && typeIds.Length > 0)
        {
            query = query.Where(m => typeIds.Contains(m.Type.Id) && m.IsPublished);
        }

        if (genreIds != null && genreIds.Length > 0)
        {
            query = query.Where(m => m.Genres.Any(g => genreIds.Contains(g.Id)) && m.IsPublished);
        }

        if (statusIds != null && statusIds.Length > 0)
        {
            query = query.Where(m => statusIds.Contains(m.Status.Id) && m.IsPublished);
        }

        query = query.Skip(skip).Take(take);

        return await query.Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type).ToListAsync();
    }

    public async Task<IEnumerable<MangaEntity>?> GetRandomAsync(int take = 1)
    {
        int totalCount = await _context.Set<MangaEntity>().CountAsync(m => m.IsPublished);

        if (totalCount == 0)
        {
            return null;
        }

        // Ensure the offset doesn't exceed the available entities
        int offset = new Random().Next(0, Math.Max(totalCount - take, 0));

        return await _context.Set<MangaEntity>()
                             .Where(m => m.IsPublished)
                             .OrderBy(m => m.Id)
                             .Skip(offset)
                             .Take(take)
                             .Include(m => m.Authors)
                             .Include(m => m.Genres)
                             .Include(m => m.Ratings)
                             .Include(m => m.CoverImage)
                             .Include(m => m.Status)
                             .Include(m => m.Type)
                             .ToListAsync();
    }



    public async Task<IEnumerable<MangaEntity>?> GetSimilarToAsync(Guid mangaId, int take = 10)
    {
        var manga = await _context.Set<MangaEntity>().FirstOrDefaultAsync(x => x.Id == mangaId);
        if (manga == null)
        {
            return null;
        }

        var mangaGenres = manga.Genres.Select(g => g.Id).ToList();

        var similarMangas = await _context.Set<MangaEntity>()
            .Where(x => x.Id != mangaId && x.IsPublished)
            .Include(m => m.Authors)
            .Include(m => m.Genres)
            .Include(m => m.Ratings)
            .Include(m => m.CoverImage)
            .Include(x => x.Status)
            .Include(x => x.Type)
            .Select(x => new
            {
                Manga = x,
                SharedGenreCount = x.Genres.Count(g => mangaGenres.Contains(g.Id))
            })
            .OrderByDescending(x => x.SharedGenreCount)
            .Take(take)
            .ToListAsync();

        return similarMangas.Select(x => x.Manga);
    }




    public async Task<IEnumerable<MangaEntity>?> SearchByTitleAsync(string title, int take = 5)
    {
        return await _context.Set<MangaEntity>()
                            .Where(x => x.Title.ToLower().Contains(title.ToLower()))
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .Take(take)
                            .ToListAsync();
    }

    public async Task<MangaEntity?> GetBySlugAsync(string slug)
    {
        return await _context.Set<MangaEntity>()
                            .Where(x => x.Slug == slug)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Authors)
                            .Include(m => m.Genres)
                            .Include(m => m.Ratings)
                            .Include(m => m.CoverImage)
                            .Include(x => x.Status)
                            .Include(x => x.Type)
                            .Include(x => x.Chapters)
                            .FirstOrDefaultAsync();
    }
}