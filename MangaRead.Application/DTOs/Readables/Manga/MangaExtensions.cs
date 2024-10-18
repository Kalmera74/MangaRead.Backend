using MangaRead.Application.DTOs.Badge;
using MangaRead.Application.DTOs.Readables.Manga;
using MangaRead.Application.DTOs.Readables.Manga.Chapter;
using MangaRead.Domain.Entities.Readables.Manga;

namespace MangaRead.Application.DTOs.Readables.Manga;
public static class MangaExtensions
{
    public static MangaDTO ToDTO(this MangaEntity mangaEntity)
    {
        var badges = new List<BadgeDTO>();
        var timeDifference = mangaEntity.CreatedAt - DateTime.Now;
        if (timeDifference <= TimeSpan.FromDays(7))
        {
            badges.Add(new BadgeDTO
            (
                nameof(BadgeType.New)
            ));
        }

        if (timeDifference <= TimeSpan.FromDays(3))
        {
            badges.Add(new BadgeDTO
            (
                nameof(BadgeType.Hot)
            ));
        }


        return new MangaDTO
        (
            mangaEntity.Id,
            mangaEntity.Title,
            mangaEntity.Description,
            mangaEntity.CoverImage.Url,
            mangaEntity.Slug,
            mangaEntity.ViewCount,
            mangaEntity.Rating,
            mangaEntity.SeasonCount,
            mangaEntity.IsPublished,
            mangaEntity.Chapters.Count > 0 ? mangaEntity.Chapters.ToDTO().ToArray() : null,
            badges.ToArray(),
            mangaEntity.Genres.Select(x => new MangaNavigationItem(x.Id, x.Name)).ToArray(),
            new MangaNavigationItem(mangaEntity.Status.Id, mangaEntity.Status.Name),
            new MangaNavigationItem(mangaEntity.Type.Id, mangaEntity.Type.Name),
            mangaEntity.Authors.Select(x => new MangaNavigationItem(x.Id, x.Name)).ToArray()
        );
    }

    public static SimpleMangaDTO ToSimpleDTO(this MangaEntity mangaEntity)
    {
        return new SimpleMangaDTO
        (
            mangaEntity.Id,
            mangaEntity.Title,
            mangaEntity.Slug,
            mangaEntity.Description,
            mangaEntity.CoverImage.Url,
            mangaEntity.Rating,
            mangaEntity.IsPublished,
            mangaEntity.Genres.Select(x => new MangaNavigationItem(x.Id, x.Name)).ToArray(),
            new MangaNavigationItem(mangaEntity.Status.Id, mangaEntity.Status.Name),
            new MangaNavigationItem(mangaEntity.Type.Id, mangaEntity.Type.Name),
            mangaEntity.Authors.Select(x => new MangaNavigationItem(x.Id, x.Name)).ToArray(),
            mangaEntity.ViewCount,
            mangaEntity.SeasonCount
        );
    }

    public static IEnumerable<SimpleMangaDTO> ToSimpleDTO(this IEnumerable<MangaEntity> mangaEntities)
    {
        return mangaEntities.Select(x => x.ToSimpleDTO());
    }
    public static IEnumerable<MangaDTO> ToDTO(this IEnumerable<MangaEntity> mangaEntities)
    {
        return mangaEntities.Select(x => x.ToDTO());
    }
}