using MangaRead.Domain.Entities.Genre;

namespace MangaRead.Application.DTOs.Genre;
public static class GenreExtensions
{

    public static GenreDTO ToDTO(this GenreEntity genreEntity)
    {
        return new GenreDTO
        (
            genreEntity.Id,
            genreEntity.Name,
            genreEntity.Slug

        );
    }

    public static IEnumerable<GenreDTO> ToDTO(this IEnumerable<GenreEntity> genreEntities)
    {
        return genreEntities.Select(x => x.ToDTO());
    }


}