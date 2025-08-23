using MangaRead.Application.DTOs.Author;
using MangaRead.Application.Repositories.Author;

public class AuthorService:IAuthorService
{

    private readonly IAuthorRepository _authorRepository;
    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AuthorDTO?> GetAuthorById(Guid id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        return author?.ToDTO();
    }


}