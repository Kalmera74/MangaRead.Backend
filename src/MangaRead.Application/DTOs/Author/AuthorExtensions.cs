using MangaRead.Domain.Entities.Author;

namespace MangaRead.Application.DTOs.Author;
public static class AuthorExtensions
{

    public static AuthorDTO ToDTO(this AuthorEntity author)
    {
        return new AuthorDTO
        (
            author.Id,
            author.Name,
            author.Slug
        );
    }


    public static IEnumerable<AuthorDTO> ToDTO(this IEnumerable<AuthorEntity> authors)
    {
        return authors.Select(a => a.ToDTO());
    }

}