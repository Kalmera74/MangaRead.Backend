using MangaRead.Application.DTOs.Author;
using MangaRead.Application.DTOs.Readables.Manga;
using MangaRead.Application.DTOs.Readables.WebNovel;
using MangaRead.Application.Repositories.Author;
using MangaRead.Application.Repositories.Readables.Manga;
using MangaRead.Application.Repositories.Readables.WebNovel;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Author;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MangaRead.API.Controllers.Author;
[Route("/api/v1/")]
[ApiController]
public class AuthorController : ControllerBase
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMangaRepository _mangaRepository;
    private readonly IWebNovelRepository _webNovelRepository;

    public AuthorController(IUnitOfWork unitOfWork, IAuthorRepository authorRepository, IMangaRepository mangaRepository, IWebNovelRepository webNovelRepository)
    {
        _unitOfWork = unitOfWork;
        _authorRepository = authorRepository;
        _mangaRepository = mangaRepository;
        _webNovelRepository = webNovelRepository;

    }

    [HttpGet("authors")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AuthorDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAllAuthors()
    {
        var authors = await _authorRepository.GetAllAsync();

        if (authors == null || !authors.Any())
        {
            return NotFound("No Authors Found");
        }

        return Ok(authors.ToDTO());

    }

    [HttpGet("authors/page/{page:int}/size/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AuthorDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAllAuthorsPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }
        var authors = await _authorRepository.GetAllPagedAsync(page, pageSize);
        if (authors == null || !authors.Any())
        {
            return NotFound("No Authors Found");
        }

        return Ok(authors.ToDTO());
    }

    [HttpGet("authors/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthorDTO>> GetAuthorById(Guid id)
    {
        var author = await _authorRepository.GetByIdAsync(id);

        if (author == null)
        {
            return NotFound($"No Author With Id: {id} Found");
        }
        return Ok(author.ToDTO());
    }

    [HttpDelete("authors/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAuthor(Guid id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null)
        {
            return NotFound($"No Author With Id: {id} Found");
        }

        await _authorRepository.DeleteAsync(author);
        await _unitOfWork.CommitAsync();

        return Accepted();
    }

    [HttpGet("authors/named/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthorDTO>> GetAuthorByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Invalid Name Parameter");
        }

        var author = await _authorRepository.GetByNameAsync(name);
        if (author == null)
        {
            return NotFound($"No Author With Name: {name} Found");
        }
        return Ok(author.ToDTO());
    }


    [HttpGet("authors/slugged/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthorDTO>> GetAuthorBySlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest("Invalid Slug Parameter");
        }

        var author = await _authorRepository.GetBySlugAsync(slug);
        if (author == null)
        {
            return NotFound($"No Author With Slug: {slug} Found");
        }

        return Ok(author.ToDTO());
    }

    [HttpPost("authors/search")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthorDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AuthorDTO>>> SearchAuthorByName([FromBody] AuthorSearchDTO author)
    {
        if (string.IsNullOrEmpty(author.Name))
        {
            return BadRequest($"Invalid Search Criteria: {nameof(author.Name)}  Is Null Or Empty");
        }

        var authors = await _authorRepository.SearchByNameAsync(author.Name);

        if (authors == null || !authors.Any())
        {
            return NotFound($"No Author With The Given Criteria Found");
        }

        return Ok(authors.ToDTO());
    }



    [HttpPost("authors")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthorDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthorDTO>> CreateAuthor([FromBody] AuthorCreateDTO author)
    {

        if (string.IsNullOrEmpty(author.Name))
        {
            return BadRequest($"Invalid Parameter: {nameof(author.Name)} Is Null Or Empty");
        }

        var existingAuthor = await _authorRepository.GetBySlugAsync(author.Name.ToSlug());
        if (existingAuthor != null)
        {
            return Ok(existingAuthor.ToDTO());
        }

        try
        {
            AuthorEntity newAuthor = AuthorEntity.Create(author.Name, author.Name.ToSlug());

            await _authorRepository.AddAsync(newAuthor);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetAuthorById), new { id = newAuthor.Id }, newAuthor.ToDTO());
        }
        catch (DuplicateEntityException ex)
        {
            Log.Error(ex, $"Tried to create an author with name: {author.Name} that already exists");
            existingAuthor = await _authorRepository.GetBySlugAsync(author.Name.ToSlug());
            if (existingAuthor == null)
            {
                return BadRequest(ex.Message);
            }
            return Ok(existingAuthor.ToDTO());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }


    }

    [HttpPut("authors/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthorDTO>> UpdateAuthor(Guid id, [FromBody] AuthorUpdateDTO author)
    {
        AuthorEntity? authorToUpdate = await _authorRepository.GetByIdAsync(id);

        if (authorToUpdate == null)
        {
            return NotFound($"No Author With Id: {id} Found");
        }

        try
        {
            if (author.Name != null)
            {
                authorToUpdate.SetName(author.Name);
                authorToUpdate.SetSlug(author.Name.ToSlug());
            }

            if (author.Mangas != null)
            {
                foreach (var mangaId in author.Mangas)
                {
                    var manga = await _mangaRepository.GetByIdAsync(mangaId);

                    if (manga == null)
                    {
                        return NotFound($"No Manga With Id: {mangaId} Found");
                    }

                    _mangaRepository.MarkAsModified(manga);
                    manga.AddAuthor(authorToUpdate);
                }
            }

            if (author.WebNovels != null)
            {
                foreach (var webNovelId in author.WebNovels)
                {
                    var webNovel = await _webNovelRepository.GetByIdAsync(webNovelId);

                    if (webNovel == null)
                    {
                        return NotFound($"No WebNovel With Id: {webNovelId} Found");
                    }

                    webNovel.AddAuthor(authorToUpdate);
                    _webNovelRepository.MarkAsModified(webNovel);
                }
            }

            _authorRepository.MarkAsModified(authorToUpdate);
            await _unitOfWork.CommitAsync();

            return Ok(authorToUpdate.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("authors/{id:guid}/mangas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MangaDTO>>> GetAuthorMangas(Guid id)
    {
        var mangas = await _mangaRepository.GetByAuthorAsync(id);

        if (mangas == null || !mangas.Any())
        {
            return NotFound($"No Manga With Author Id: {id} Found");
        }
        return Ok(mangas.ToDTO());
    }

    [HttpGet("authors/{id:guid}/webNovels")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<ActionResult<IEnumerable<WebNovelDTO>>> GetAuthorWebNovels(Guid id)
    {
        var webNovels = await _webNovelRepository.GetByAuthorAsync(id);

        if (webNovels == null || !webNovels.Any())
        {
            return NotFound($"No WebNovel With Author Id: {id} Found");
        }
        return Ok(webNovels.ToDTO());
    }

}