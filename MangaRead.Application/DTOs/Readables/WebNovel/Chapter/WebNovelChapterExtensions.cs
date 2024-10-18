using MangaRead.Domain.Entities.Readables.WebNovel.Chapter;


namespace MangaRead.Application.DTOs.Readables.WebNovel.Chapter;
public static class WebNovelChapterExtensions
{
    public static IEnumerable<WebNovelChapterDTO> ToDTO(this IEnumerable<WebNovelChapterEntity> webNovelChapterEntities)
    {
        return webNovelChapterEntities.Select(x => x.ToDTO());
    }
    public static WebNovelChapterDTO ToDTO(this WebNovelChapterEntity webNovelChapterEntities)
    {
        return new WebNovelChapterDTO
        (
            webNovelChapterEntities.Id,
            webNovelChapterEntities.Title,
            webNovelChapterEntities.Slug,
            webNovelChapterEntities.PreviousChapter?.Id,
            webNovelChapterEntities.NextChapter?.Id,
            webNovelChapterEntities.Content?.Id,
            webNovelChapterEntities.WebNovel.Id
        );
    }
}