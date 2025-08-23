using MangaRead.Domain.Entities.Readables.Manga.Type;

namespace MangaRead.Application.DTOs.Readables.Manga.Type;

public static class MangaTypeExtensions
{
    public static IEnumerable<MangaTypeDTO> ToDTO(this IEnumerable<MangaTypeEntity> mangaTypes)
    {
        return mangaTypes.Select(m => m.ToDTO());
    }

    public static MangaTypeDTO ToDTO(this MangaTypeEntity mangaType)
    {
        return new MangaTypeDTO
        (
            mangaType.Id,
            mangaType.Name,
            mangaType.Slug

        );
    }
}