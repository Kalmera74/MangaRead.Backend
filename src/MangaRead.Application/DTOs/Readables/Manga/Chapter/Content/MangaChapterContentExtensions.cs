using MangaRead.Application.DTOs.Readables.Manga.Chapter.Content;
using MangaRead.Application.DTOs.System.Image;
using MangaRead.Domain.Entities.Readables.Manga.Chapter.Content;

namespace MangaRead.Application.DTOs.Readables.Manga.Chapter.Content;
public static class MangaChapterContentExtensions
{

    public static MangaChapterContentDTO ToDTO(this MangaChapterContentEntity mangaChapterContentEntity)
    {
        return new MangaChapterContentDTO
        (
            mangaChapterContentEntity.Id,
            mangaChapterContentEntity.Item.ToDTO()
        );
    }

    public static IEnumerable<MangaChapterContentDTO> ToDTO(this IEnumerable<MangaChapterContentEntity> mangaChapterContentEntities)
    {
        return mangaChapterContentEntities.Select(x => x.ToDTO());
    }
}