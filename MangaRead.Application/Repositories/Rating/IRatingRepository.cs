using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Rating;

namespace MangaRead.Application.Repositories.Rating;
public interface IRatingRepository : IRepository<RatingEntity>
{

    public Task<IEnumerable<RatingEntity>> GetByUserAsync(Guid userId);
    public Task<IEnumerable<RatingEntity>> GetByUserPagedAsync(Guid userId, int page, int pageSize);

    public Task<IEnumerable<RatingEntity>> GetByStarCountAsync(float minStarCount, float maxStarCount);
}
