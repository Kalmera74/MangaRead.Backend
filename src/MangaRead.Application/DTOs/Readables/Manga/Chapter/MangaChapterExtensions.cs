using MangaRead.Application.DTOs.Readables.Manga.Chapter;
using MangaRead.Application.DTOs.Readables.Manga.Chapter.Content;
using MangaRead.Domain.Entities.Readables.Manga.Chapter;

namespace MangaRead.Application.DTOs.Readables.Manga.Chapter;
public static class MangaChapterExtensions
{
    public static IEnumerable<MangaChapterDTO> ToDTO(this IEnumerable<MangaChapterEntity> mangaChapterEntities)
    {
        return mangaChapterEntities.Select(x => x.ToDTO());
    }

    public static IEnumerable<SimpleMangaChapterDTO> ToSimpleDTO(this IEnumerable<MangaChapterEntity> mangaChapterEntities)
    {
        return mangaChapterEntities.Select(x => x.ToSimpleDTO());
    }

    public static SimpleMangaChapterDTO ToSimpleDTO(this MangaChapterEntity mangaChapterEntity)
    {
        return new SimpleMangaChapterDTO
        (
            mangaChapterEntity.Id,
            mangaChapterEntity.Title,
            mangaChapterEntity.Slug,
            mangaChapterEntity.IsPublished,
            mangaChapterEntity.Manga.Id
        );
    }

    public static MangaChapterDTO ToDTO(this MangaChapterEntity mangaChapterEntity)
    {
        return new MangaChapterDTO
        (
            mangaChapterEntity.Id,
            mangaChapterEntity.Title,
            mangaChapterEntity.Slug,
            mangaChapterEntity.IsPublished,
            mangaChapterEntity.MetaTitle,
            mangaChapterEntity.MetaDescription,
            mangaChapterEntity.PreviousChapter?.Id,
            mangaChapterEntity.NextChapter?.Id,
            mangaChapterEntity.Content.Select(x => x.ToDTO()).ToArray(),
            mangaChapterEntity.Manga.Id
        );
    }

}