using MangaRead.Domain.Entities.Readables.WebNovel.Chapter.Content;

namespace MangaRead.Application.DTOs.Readables.WebNovel.Chapter.Content;
public static class WebNovelChapterContentExtensions
{

    public static WebNovelChapterContentDTO ToDTO(this WebNovelChapterContentEntity webNovelChapterContentEntity)
    {
        return new WebNovelChapterContentDTO
        (
            webNovelChapterContentEntity.Id,
            webNovelChapterContentEntity.Title,
            webNovelChapterContentEntity.MetaTitle,
            webNovelChapterContentEntity.MetaDescription,
            webNovelChapterContentEntity.Body

        );
    }

    public static IEnumerable<WebNovelChapterContentDTO> ToDTO(this IEnumerable<WebNovelChapterContentEntity> webNovelChapterContentEntities)
    {
        return webNovelChapterContentEntities.Select(x => x.ToDTO());
    }
}