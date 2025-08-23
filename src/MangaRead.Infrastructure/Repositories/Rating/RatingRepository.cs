using MangaRead.Application.Repositories.Rating;
using MangaRead.Domain.Entities.Rating;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaRead.Infrastructure.Repositories.Rating;
public class RatingRepository : Repository<RatingEntity>, IRatingRepository
{
    public RatingRepository(MangaDbContext context) : base(context)
    {
    }



    public async Task<IEnumerable<RatingEntity>> GetByStarCountAsync(float minStarCount, float maxStarCount)
    {
        return await _context.Set<RatingEntity>().Where(x => x.StarCount >= minStarCount && x.StarCount <= maxStarCount).ToListAsync();
    }


    public async Task<IEnumerable<RatingEntity>> GetByUserAsync(Guid userId)
    {
        return await _context.Set<RatingEntity>().Where(x => x.User.Id == userId).ToListAsync();
    }

    public async Task<IEnumerable<RatingEntity>> GetByUserPagedAsync(Guid userId, int page, int pageSize)
    {
        return await _context.Set<RatingEntity>().Where(x => x.User.Id == userId).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }
}