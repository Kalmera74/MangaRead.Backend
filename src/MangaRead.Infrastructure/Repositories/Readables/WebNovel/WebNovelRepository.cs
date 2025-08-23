using MangaRead.Application.Repositories.Readables.WebNovel;
using MangaRead.Domain.Entities.Readables.WebNovel;
using MangaRead.Infrastructure.DbContexts;

namespace MangaRead.Infrastructure.Repositories.Readables.WebNovel;

public class WebNovelRepository : Repository<WebNovelEntity>, IWebNovelRepository
{
    public WebNovelRepository(MangaDbContext context) : base(context)
    {
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByAuthorAsync(Guid authorId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByAuthorsAsync(Guid[] authorIds)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByCreatedAtAsync(DateTime date, int take = 10)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByGenreAsync(Guid genreId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByGenresAsync(Guid[] genreIds)
    {
        throw new NotImplementedException();
    }

    public Task<WebNovelEntity?> GetByMangaAsync(Guid mangaId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByRandomAsync(int take = 10)
    {
        throw new NotImplementedException();
    }

    public Task<WebNovelEntity?> GetByRatingAsync(Guid ratingId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByRatingsAsync(int min, int max)
    {
        throw new NotImplementedException();
    }

    public Task<WebNovelEntity?> GetBySlugAsync(string slug)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByStatusAsync(Guid statusId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByStatusesAsync(Guid[] statusIds)
    {
        throw new NotImplementedException();
    }

    public Task<WebNovelEntity?> GetByTitleAsync(string title)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByTypeAsync(Guid typeId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByTypesAsync(Guid[] typeIds)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByUpdatedAtAsync(DateTime date, int take = 10)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetByViewsAsync(int take = 10)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetFilteredAsync(Guid[]? genreIds, Guid[]? statusIds)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetFilteredPagedAsync(Guid[]? genreIds, Guid[]? statusIds, int skip, int take)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetRandomAsync(int take = 1)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> GetSimilarToAsync(Guid mangaId, int take = 10)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WebNovelEntity>?> SearchByTitleAsync(string title)
    {
        throw new NotImplementedException();
    }
}