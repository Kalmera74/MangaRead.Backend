using MangaRead.Domain.Entities.Readables.WebNovel.Type;

namespace MangaRead.Application.DTOs.Readables.WebNovel.Type;

public static class WebNovelTypeExtensions
{
    public static IEnumerable<WebNovelTypeDTO> ToDTO(this IEnumerable<WebNovelTypeEntity> mangaTypes)
    {
        return mangaTypes.Select(m => m.ToDTO());
    }

    public static WebNovelTypeDTO ToDTO(this WebNovelTypeEntity mangaType)
    {
        return new WebNovelTypeDTO
        (
            mangaType.Id,
            mangaType.Name,
            mangaType.Slug

        );
    }
}