using MangaRead.Application.DTOs.Genre;
using MangaRead.Application.DTOs.Readables.Manga;
using MangaRead.Application.Repositories.Genre;
using MangaRead.Application.Repositories.Readables.Manga;
using MangaRead.Application.Repositories.Readables.WebNovel;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Genre;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace MangaRead.API.Controllers.Genre;

[Route("/api/v1/")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenreRepository _genreRepository;
    private readonly IMangaRepository _mangaRepository;
    private readonly IWebNovelRepository _webNovelRepository;
    public GenreController(IUnitOfWork unitOfWork, IGenreRepository mangaGenreRepository, IMangaRepository mangaRepository, IWebNovelRepository webNovelRepository)
    {
        _unitOfWork = unitOfWork;
        _genreRepository = mangaGenreRepository;
        _mangaRepository = mangaRepository;
        _webNovelRepository = webNovelRepository;
    }
    [HttpGet("genres")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GenreDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres()
    {
        var genres = await _genreRepository.GetAllAsync();
        if (genres == null || !genres.Any())
        {
            return NotFound("No Genres Found");
        }
        return Ok(genres.ToDTO());
    }

    [HttpGet("genres/page/{page:int}/size/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GenreDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenresPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }

        var genres = await _genreRepository.GetAllPagedAsync(page, pageSize);
        if (genres == null || !genres.Any())
        {
            return NotFound("No Genres Found");
        }
        return Ok(genres.ToDTO());
    }

    [HttpGet("genres/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenreDTO>> GetGenreById(Guid id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre == null)
        {
            return NotFound($"No Genre With Id: {id} Found");
        }
        return Ok(genre.ToDTO());
    }

    [HttpGet("genres/named/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenreDTO>> GetGenreByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest($"Invalid Name Parameter");
        }

        var genre = await _genreRepository.GetByNameAsync(name);
        if (genre == null)
        {
            return NotFound($"No Genre With Name: {name} Found");
        }
        return Ok(genre.ToDTO());
    }

    [HttpGet("genres/slugged/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenreDTO>> GetGenreBySlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest("Invalid Slug Parameter");
        }

        var genre = await _genreRepository.GetBySlugAsync(slug);
        if (genre == null)
        {
            return NotFound($"No Genre With Slug: {slug} Found");
        }

        return Ok(genre.ToDTO());
    }

    [HttpGet("genres/{id:guid}/mangas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<MangaDTO>>> GetGenreMangas(Guid id)
    {
        var mangas = await _mangaRepository.GetByGenreAsync(id);

        if (mangas == null || !mangas.Any())
        {
            return NoContent();
        }

        return Ok(mangas.ToDTO());
    }


    [HttpPost("genres/search")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenreDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<GenreDTO>>> SearchGenreByName([FromBody] GenreSearchDTO genre)
    {
        if (string.IsNullOrEmpty(genre.Name))
        {
            return BadRequest($"Invalid Parameter: {nameof(genre.Name)} Is Null Or Empty");
        }

        var genres = await _genreRepository.SearchByNameAsync(genre.Name);

        if (genres == null || !genres.Any())
        {
            return NotFound("No Genres With The Given Criteria Found");
        }

        return Ok(genres.ToDTO());
    }

    [HttpPost("genres")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenreDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenreDTO>> CreateGenre([FromBody] GenreCreateDTO genre)
    {

        if (string.IsNullOrEmpty(genre.Name))
        {
            return BadRequest($"Invalid Parameter: {nameof(genre.Name)} Is Null Or Empty");
        }

        var existingGenre = await _genreRepository.GetBySlugAsync(genre.Name.ToSlug());
        if (existingGenre != null)
        {
            return Ok(existingGenre.ToDTO());
        }

        try
        {
            GenreEntity newGenre = GenreEntity.Create(genre.Name, genre.Name.ToSlug());

            await _genreRepository.AddAsync(newGenre);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetGenreById), new { id = newGenre.Id }, newGenre.ToDTO());
        }
        catch (DuplicateEntityException ex)
        {
            Log.Error(ex, $"Tried to create a genre with name: {genre.Name} that already exists");
            existingGenre = await _genreRepository.GetBySlugAsync(genre.Name.ToSlug());
            if (existingGenre == null)
            {
                return BadRequest(ex.Message);
            }
            return Ok(existingGenre.ToDTO());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("genres/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenreDTO>> UpdateGenre(Guid id, [FromBody] GenreUpdateDTO genre)
    {

        GenreEntity? genreToUpdate = await _genreRepository.GetByIdAsync(id);

        if (genreToUpdate == null)
        {
            return NotFound($"No Genre With Id: {id} Found");
        }

        try
        {
            if (genre.Name != null)
            {
                genreToUpdate.SetName(genre.Name);
                genreToUpdate.SetSlug(genre.Name.ToSlug());
            }

            _genreRepository.MarkAsModified(genreToUpdate);
            await _unitOfWork.CommitAsync();

            return Ok(genreToUpdate.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }

    }

}