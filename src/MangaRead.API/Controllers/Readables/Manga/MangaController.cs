using MangaRead.Application.DTOs.Readables.Manga;
using MangaRead.Application.DTOs.Readables.WebNovel;
using MangaRead.Application.Repositories.Author;
using MangaRead.Application.Repositories.Genre;
using MangaRead.Application.Repositories.Readables.Manga;
using MangaRead.Application.Repositories.Readables.Manga.Type;
using MangaRead.Application.Repositories.Readables.WebNovel;
using MangaRead.Application.Repositories.Status;
using MangaRead.Application.Repositories.System.Image;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Author;
using MangaRead.Domain.Entities.Genre;
using MangaRead.Domain.Entities.Readables.Manga;
using Microsoft.AspNetCore.Mvc;
using Serilog;
namespace MangaRead.API.Controllers.Readables.Manga;

[Route("/api/v1/")]
[ApiController]

public class MangaController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMangaRepository _mangaRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IStatusRepository _mangaStatusRepository;
    private readonly IMangaTypeRepository _mangaTypeRepository;
    private readonly IGenreRepository _mangaGenreRepository;
    private readonly IWebNovelRepository _webNovelRepository;

    public MangaController(
        IUnitOfWork unitOfWork,
        IMangaRepository mangaRepository,
        IAuthorRepository authorRepository,
        IImageRepository imageRepository,
        IStatusRepository mangaStatusRepository,
        IMangaTypeRepository mangaTypeRepository,
        IGenreRepository mangaGenreRepository,
        IWebNovelRepository webNovelRepository
    )
    {
        _unitOfWork = unitOfWork;
        _mangaRepository = mangaRepository;
        _authorRepository = authorRepository;
        _imageRepository = imageRepository;
        _mangaStatusRepository = mangaStatusRepository;
        _mangaTypeRepository = mangaTypeRepository;
        _mangaGenreRepository = mangaGenreRepository;
        _webNovelRepository = webNovelRepository;
    }

    [HttpGet("mangas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SimpleMangaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<SimpleMangaDTO>>> GetMangas()
    {
        var mangas = await _mangaRepository.GetAllAsync();
        if (mangas == null || !mangas.Any())
        {
            return NotFound("No Mangas Found");
        }
        return Ok(mangas.ToSimpleDTO());
    }

    [HttpGet("mangas/page/{page}/size/{pageSize}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MangaDTO>>> GetMangasPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }
        var mangas = await _mangaRepository.GetAllPagedAsync(page, pageSize);
        if (mangas == null || !mangas.Any())
        {
            return NotFound("No Mangas Found");
        }
        return Ok(mangas.ToDTO());
    }

    [HttpGet("mangas/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MangaDTO>> GetManga(Guid id)
    {
        var manga = await _mangaRepository.GetByIdAsync(id);
        if (manga == null)
        {
            return NotFound($"No Manga With Id: {id} Found");
        }
        return Ok(manga.ToDTO());
    }



    [HttpGet("mangas/slugged/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MangaDTO>> GetMangaBySlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest("Invalid Slug Parameter");
        }

        var manga = await _mangaRepository.GetBySlugAsync(slug);
        if (manga == null)
        {
            return NotFound($"No Manga With Slug: {slug} Found");
        }

        return Ok(manga.ToDTO());
    }





    [HttpGet("mangas/{id}/similar/mangas/{take}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SimpleMangaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<SimpleMangaDTO>> GetSimilarMangas(Guid id, int take)
    {
        if (take < 1)
        {
            return BadRequest("Invalid Take Value, Cannot Be Less Than 1");
        }

        var manga = await _mangaRepository.GetByIdAsync(id);
        if (manga == null)
        {
            return NotFound($"No Manga With Id: {id} Found");
        }

        var mangas = await _mangaRepository.GetSimilarToAsync(id, take);
        if (mangas == null || !mangas.Any())
        {
            return NoContent();
        }
        return Ok(mangas.ToSimpleDTO());
    }


    [HttpGet("mangas/new/{take}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SimpleMangaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<SimpleMangaDTO>>> GetNewMangas(int take)
    {
        if (take < 1)
        {
            return BadRequest("Invalid Take Value, Cannot Be Less Than 1");
        }

        var mangas = await _mangaRepository.GetByCreatedAtAsync(DateTime.Now.AddYears(-7), DateTime.Now, take);
        if (mangas == null || !mangas.Any())
        {
            return NotFound("No Mangas Found");
        }
        return Ok(mangas.ToSimpleDTO());
    }

    [HttpGet("mangas/hot/{take}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SimpleMangaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<SimpleMangaDTO>>> GetHotMangas(int take)
    {
        if (take < 1)
        {
            return BadRequest("Invalid Take Value, Cannot Be Less Than 1");
        }

        var mangas = await _mangaRepository.GetByUpdatedAtAsync(DateTime.Now.AddYears(-1), DateTime.Now, take);

        if (mangas == null || !mangas.Any())
        {
            return NotFound("No Mangas Found");
        }
        return Ok(mangas.ToSimpleDTO());
    }

    [HttpGet("mangas/poster/{take}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SimpleMangaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<SimpleMangaDTO>>> GetPosterMangas(int take)
    {
        if (take < 1)
        {
            return BadRequest("Invalid Take Value, Cannot Be Less Than 1");
        }
        var mangas = await _mangaRepository.GetByRandomAsync(take);

        if (mangas == null || !mangas.Any())
        {
            return NotFound("No Mangas Found");
        }

        return Ok(mangas.ToSimpleDTO());
    }

    [HttpGet("mangas/suggested/{take}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MangaDTO>>> GetRecommendedMangas(int take)
    {
        if (take < 1)
        {
            return BadRequest("Invalid Take Value, Cannot Be Less Than 1");
        }

        var mangas = await _mangaRepository.GetRandomAsync(take);

        if (mangas == null || !mangas.Any())
        {
            return NotFound("No Mangas Found");
        }

        return Ok(mangas.ToDTO());
    }


    [HttpGet("mangas/{id}/web-novel")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<WebNovelDTO>> GetMangaWebNovel(Guid id)
    {
        var manga = await _mangaRepository.GetByIdAsync(id);

        if (manga == null)
        {
            return NotFound($"No Manga With Id: {id} Found");
        }

        var webNovel = manga.WebNovel;

        if (webNovel == null)
        {
            return NoContent();
        }
        return Ok(webNovel.ToDTO());

    }


    [HttpDelete("mangas/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteManga(Guid id)
    {
        var manga = await _mangaRepository.GetByIdAsync(id);
        if (manga == null)
        {
            return NotFound($"No Manga With Id: {id} Found");
        }

        await _mangaRepository.DeleteAsync(manga);
        await _unitOfWork.CommitAsync();
        return Accepted();
    }


    [HttpPost("mangas")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MangaDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MangaDTO>> CreateManga([FromBody] MangaCreateDTO manga)
    {
        if (string.IsNullOrEmpty(manga.Title))
        {
            return BadRequest($"Invalid Parameter: {nameof(manga.Title)} Is Null Or Empty");
        }

        var existingManga = await _mangaRepository.GetBySlugAsync(manga.Title.ToSlug());

        if (existingManga != null)
        {
            return Ok(existingManga.ToDTO());
        }

        try
        {

            var authors = new List<AuthorEntity>();
            foreach (var authorId in manga.Authors)
            {
                var author = await _authorRepository.GetByIdAsync(authorId);

                if (author == null)
                {
                    return NotFound($"No Author With Id: {authorId} Found");
                }
                authors.Add(author);

            }
            var status = await _mangaStatusRepository.GetByIdAsync(manga.Status);

            if (status == null)
            {
                return NotFound($"No Status With Id: {manga.Status} Found");
            }

            var image = await _imageRepository.GetByIdAsync(manga.CoverImage);

            if (image == null)
            {
                return NotFound($"No Image With Id: {manga.Type} Found");
            }


            List<GenreEntity> genres = new();

            foreach (var genreId in manga.Genres)
            {
                var genre = await _mangaGenreRepository.GetByIdAsync(genreId);

                if (genre == null)
                {
                    return NotFound($"No Genre With Id: {genreId} Found");
                }
                genres.Add(genre);
            }

            var type = await _mangaTypeRepository.GetByIdAsync(manga.Type);

            if (type == null)
            {
                return NotFound($"No Manga Type With Id: {manga.Type} Found");
            }



            MangaEntity newManga = MangaEntity.Create(
                manga.Title,
                manga.Title.ToSlug(),
                manga.Description,
                authors.ToArray(),
                image,
                status,
                type,
                genres);

            if (manga.WebNovel != null)
            {
                var webNovel = await _webNovelRepository.GetByIdAsync(manga.WebNovel.Value);

                if (webNovel == null)
                {
                    return NotFound($"No WebNovel With Id: {manga.WebNovel} Found");
                }

                newManga.SetWebNovel(webNovel);
                _webNovelRepository.MarkAsModified(webNovel);
            }

            if (!string.IsNullOrEmpty(manga.MetaTitle))
            {
                newManga.SetMetaTitle(manga.MetaTitle);
            }
            if (!string.IsNullOrEmpty(manga.MetaDescription))
            {
                newManga.SetMetaDescription(manga.MetaDescription);
            }

            await _mangaRepository.AddAsync(newManga);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetManga), new { id = newManga.Id }, newManga.ToDTO());
        }
        catch (DuplicateEntityException ex)
        {
            Log.Error(ex, $"Tried to create a manga with title: {manga.Title} that already exists");
            existingManga = await _mangaRepository.GetBySlugAsync(manga.Title.ToSlug());
            if (existingManga == null)
            {
                return BadRequest(ex.Message);
            }
            return Ok(existingManga.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("mangas/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MangaDTO>> UpdateManga(Guid id, [FromBody] MangaUpdateDTO manga)
    {
        var mangaToUpdate = await _mangaRepository.GetByIdAsync(id);

        if (mangaToUpdate == null)
        {
            return NotFound($"No Manga With Id: {id} Found");
        }

        try
        {
            if (manga.Title != null)
            {
                mangaToUpdate.SetTitle(manga.Title);
            }
            if (manga.Description != null)
            {
                mangaToUpdate.SetDescription(manga.Description);
            }
            if (manga.CoverImage != null)
            {
                var image = await _imageRepository.GetByIdAsync(manga.CoverImage.Value);

                if (image == null)
                {
                    return NotFound($"No Image With Url: {manga.CoverImage} Found");
                }

                mangaToUpdate.SetCoverImage(image);
            }

            if (manga.Genres != null)
            {
                if (manga.Genres.Count() < 1)
                {
                    return BadRequest("Genres Cannot Be Empty");
                }

                foreach (var genreId in manga.Genres)
                {
                    var genre = await _mangaGenreRepository.GetByIdAsync(genreId);

                    if (genre == null)
                    {
                        return NotFound($"No Manga Genre With Id: {genreId} Found");
                    }

                    mangaToUpdate.AddGenre(genre);
                }
            }

            if (manga.Status != null)
            {
                var status = await _mangaStatusRepository.GetByIdAsync(manga.Status.Value);

                if (status == null)
                {
                    return NotFound($"No Manga Status With Id: {manga.Status} Found");
                }
                mangaToUpdate.SetStatus(status);
            }

            if (manga.Type != null)
            {
                var type = await _mangaTypeRepository.GetByIdAsync(manga.Type.Value);

                if (type == null)
                {
                    return NotFound($"No Manga Type With Id: {manga.Type} Found");
                }

                mangaToUpdate.SetType(type);
            }

            if (manga.Authors != null)
            {
                foreach (var authorId in manga.Authors)
                {

                    var author = await _authorRepository.GetByIdAsync(authorId);

                    if (author == null)
                    {
                        return NotFound($"No Author With Id: {authorId} Found");
                    }

                    mangaToUpdate.AddAuthor(author);
                }
            }

            if (manga.WebNovel != null)
            {
                var webNovel = await _webNovelRepository.GetByIdAsync(manga.WebNovel.Value);
                if (webNovel == null)
                {
                    return NotFound($"No WebNovel With Id: {manga.WebNovel} Found");
                }
                mangaToUpdate.SetWebNovel(webNovel);
                _webNovelRepository.MarkAsModified(webNovel);
            }

            _mangaRepository.MarkAsModified(mangaToUpdate);
            await _unitOfWork.CommitAsync();

            return Ok(mangaToUpdate.ToDTO());

        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return BadRequest(e.Message);
        }
    }


    [HttpPost("mangas/search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SimpleMangaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SimpleMangaDTO>> SearchMangas([FromBody] MangaSearchDTO manga)
    {
        if (string.IsNullOrWhiteSpace(manga.Title))
        {
            return BadRequest($"Invalid Parameter {nameof(manga.Title)} Cannot Be Empty or Null");
        }

        var mangas = await _mangaRepository.SearchByTitleAsync(manga.Title);

        if (mangas == null || !mangas.Any())
        {
            return NotFound($"No Manga With The Given Criteria Found");
        }
        return Ok(mangas.ToDTO());
    }

    [HttpPost("mangas/filtered")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SimpleMangaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SimpleMangaDTO>> GetMangasFiltered([FromBody] MangaFilterDTO manga)
    {

        if (manga.Genres == null && manga.Types == null && manga.Status == null)
        {
            return BadRequest("No Filter Criteria Given");
        }


        var mangas = await _mangaRepository.GetFilteredMangasAsync(manga.Genres, manga.Types, manga.Status);

        if (mangas == null || !mangas.Any())
        {
            return NotFound($"No Manga With The Given Criteria Found");
        }

        return Ok(mangas.ToDTO());
    }


    [HttpPost("mangas/filtered/page/{page}/size/{pageSize}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SimpleMangaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<SimpleMangaDTO>>> GetMangasFilteredPaginated([FromBody] MangaFilterDTO manga, int page, int pageSize)
    {
        if (manga.Genres == null && manga.Types == null && manga.Status == null)
        {
            return BadRequest("No Filter Criteria Given");
        }

        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }


        var mangas = await _mangaRepository.GetFilteredMangasPagedAsync(manga.Genres, manga.Types, manga.Status, page, pageSize);

        if (mangas == null || !mangas.Any())
        {
            return NotFound($"No Manga With The Given Criteria Found");
        }

        return Ok(mangas.ToDTO());
    }



}