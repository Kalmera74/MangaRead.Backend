using MangaRead.Application.Repositories;
using MangaRead.Domain.Entities.Readables.WebNovel;

namespace MangaRead.Application.Repositories.Readables.WebNovel;
public interface IWebNovelRepository : IRepository<WebNovelEntity>
{

    public Task<WebNovelEntity?> GetByTitleAsync(string title);
    public Task<IEnumerable<WebNovelEntity>?> GetSimilarToAsync(Guid mangaId, int take = 10);
    public Task<IEnumerable<WebNovelEntity>?> SearchByTitleAsync(string title);

    public Task<IEnumerable<WebNovelEntity>?> GetByCreatedAtAsync(DateTime date, int take = 10);

    public Task<IEnumerable<WebNovelEntity>?> GetByUpdatedAtAsync(DateTime date, int take = 10);

    public Task<IEnumerable<WebNovelEntity>?> GetByViewsAsync(int take = 10);

    public Task<IEnumerable<WebNovelEntity>?> GetRandomAsync(int take = 1);
    public Task<WebNovelEntity?> GetBySlugAsync(string slug);
    public Task<WebNovelEntity?> GetByRatingAsync(Guid ratingId);
    public Task<IEnumerable<WebNovelEntity>?> GetByRatingsAsync(int min, int max);
    public Task<IEnumerable<WebNovelEntity>?> GetByRandomAsync(int take = 10);

    public Task<IEnumerable<WebNovelEntity>?> GetByTypeAsync(Guid typeId);
    public Task<IEnumerable<WebNovelEntity>?> GetByTypesAsync(Guid[] typeIds);

    public Task<IEnumerable<WebNovelEntity>?> GetByGenreAsync(Guid genreId);
    public Task<IEnumerable<WebNovelEntity>?> GetByGenresAsync(Guid[] genreIds);

    public Task<IEnumerable<WebNovelEntity>?> GetByStatusAsync(Guid statusId);
    public Task<IEnumerable<WebNovelEntity>?> GetByStatusesAsync(Guid[] statusIds);

    public Task<IEnumerable<WebNovelEntity>?> GetByAuthorAsync(Guid authorId);
    public Task<IEnumerable<WebNovelEntity>?> GetByAuthorsAsync(Guid[] authorIds);

    public Task<IEnumerable<WebNovelEntity>?> GetFilteredAsync(Guid[]? genreIds, Guid[]? statusIds);
    public Task<IEnumerable<WebNovelEntity>?> GetFilteredPagedAsync(Guid[]? genreIds, Guid[]? statusIds, int skip, int take);


    public Task<WebNovelEntity?> GetByMangaAsync(Guid mangaId);


}
