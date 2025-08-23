using MangaRead.Application.DTOs.Badge;
using MangaRead.Domain.Entities.Readables.WebNovel;

namespace MangaRead.Application.DTOs.Readables.WebNovel;
public static class WebNovelExtensions
{

    public static WebNovelDTO ToDTO(this WebNovelEntity webNovelEntity)
    {
        var badges = new List<BadgeDTO>();
        var timeDifference = webNovelEntity.CreatedAt - DateTime.Now;
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
        return new WebNovelDTO
        (
            webNovelEntity.Id,
            webNovelEntity.Title,
            webNovelEntity.Description,
            webNovelEntity.CoverImage.Url,
            webNovelEntity.Genres.Select(x => new WebNovelNavigationItem(x.Id, x.Name)).ToArray(),
            new WebNovelNavigationItem(webNovelEntity.Status.Id, webNovelEntity.Status.Name),
            webNovelEntity.Authors.Select(x => new WebNovelNavigationItem(x.Id, x.Name)).ToArray(),
            webNovelEntity.Slug,
            badges.ToArray()
        );
    }

    public static IEnumerable<WebNovelDTO> ToDTO(this IEnumerable<WebNovelEntity> webNovelEntities)
    {
        return webNovelEntities.Select(x => x.ToDTO());
    }
}