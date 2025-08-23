using MangaRead.Application.DTOs.Author;

public interface IAuthorService
{
    Task<AuthorDTO?> GetAuthorById(Guid id);
}