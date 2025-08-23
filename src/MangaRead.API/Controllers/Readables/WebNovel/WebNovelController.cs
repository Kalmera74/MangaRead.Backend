using MangaRead.Application.DTOs.Readables.Manga;
using MangaRead.Application.DTOs.Readables.WebNovel;
using MangaRead.Application.DTOs.Readables.WebNovel.Chapter;
using MangaRead.Application.Repositories.Author;
using MangaRead.Application.Repositories.Genre;
using MangaRead.Application.Repositories.Readables.Manga;
using MangaRead.Application.Repositories.Readables.WebNovel;
using MangaRead.Application.Repositories.Status;
using MangaRead.Application.Repositories.System.Image;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Author;
using MangaRead.Domain.Entities.Genre;
using MangaRead.Domain.Entities.Readables.WebNovel;
using Microsoft.AspNetCore.Mvc;
namespace MangaRead.API.Controllers.Readables.WebNovel;

[Route("/api/v1/")]
[ApiController]

public class WebNovelController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMangaRepository _mangaRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IWebNovelRepository _webNovelRepository;

    public WebNovelController(
        IUnitOfWork unitOfWork,
        IMangaRepository mangaRepository,
        IAuthorRepository authorRepository,
        IImageRepository imageRepository,
        IStatusRepository statusRepository,
        IGenreRepository genreRepository,
        IWebNovelRepository webNovelRepository
    )
    {
        _unitOfWork = unitOfWork;
        _mangaRepository = mangaRepository;
        _authorRepository = authorRepository;
        _imageRepository = imageRepository;
        _statusRepository = statusRepository;
        _genreRepository = genreRepository;
        _webNovelRepository = webNovelRepository;
    }

    [HttpGet("web-novels")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<WebNovelDTO>>> GetWebNovels()
    {
        var webNovels = await _webNovelRepository.GetAllAsync();
        if (webNovels == null || !webNovels.Any())
        {
            return NotFound("No WebNovels Found");
        }
        return Ok(webNovels.ToDTO());
    }

    [HttpGet("web-novels/page/{page}/size/{pageSize}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<WebNovelDTO>>> GetWebNovelsPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }
        var webNovels = await _webNovelRepository.GetAllPagedAsync(page, pageSize);
        if (webNovels == null || !webNovels.Any())
        {
            return NotFound("No WebNovel Found");
        }
        return Ok(webNovels.ToDTO());
    }

    [HttpGet("web-novels/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WebNovelDTO>> GetWebNovel(Guid id)
    {
        var webNovel = await _webNovelRepository.GetByIdAsync(id);
        if (webNovel == null)
        {
            return NotFound($"No WebNovel With Id: {id} Found");
        }
        return Ok(webNovel.ToDTO());
    }


    [HttpGet("web-novels/slugged/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelDTO>> GetMangaChapterBySlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest("Invalid Slug Parameter");
        }

        var webNovel = await _webNovelRepository.GetBySlugAsync(slug);
        if (webNovel == null)
        {
            return NotFound($"No WebNovel With Slug: {slug} Found");
        }

        return Ok(webNovel.ToDTO());
    }



    [HttpGet("web-novels/{id}/chapters")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelChapterDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<WebNovelChapterDTO>> GetWebNovelChapter(Guid id)
    {
        var webNovel = await _webNovelRepository.GetByIdAsync(id);
        if (webNovel == null)
        {
            return NotFound($"No WebNovel With Id: {id} Found");
        }

        var chapters = webNovel.Chapters;

        if (chapters == null || !chapters.Any())
        {
            return NoContent();
        }
        return Ok(chapters.ToDTO());
    }

    [HttpGet("web-novels/{id}/similar/web-novels/{take:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<WebNovelDTO>> GetSimilarWebNovels(Guid id, int take)
    {
        if (take < 1)
        {
            return BadRequest("Invalid Take Value, Cannot Be Less Than 1");
        }

        var webNovel = await _webNovelRepository.GetByIdAsync(id);
        if (webNovel == null)
        {
            return NotFound($"No WebNovel With Id: {id} Found");
        }

        var webNovels = await _webNovelRepository.GetSimilarToAsync(id, take);
        if (webNovels == null || !webNovels.Any())
        {
            return NoContent();
        }
        return Ok(webNovels.ToDTO());
    }


    [HttpGet("web-novels/new/{take}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<WebNovelDTO>>> GetNewWebNovels(int take)
    {
        if (take < 1)
        {
            return BadRequest("Invalid Take Value, Cannot Be Less Than 1");
        }

        var webNovels = await _webNovelRepository.GetByCreatedAtAsync(DateTime.Now, take);
        if (webNovels == null || !webNovels.Any())
        {
            return NotFound("No WebNovels Found");
        }
        return Ok(webNovels.ToDTO());
    }

    [HttpGet("web-novels/hot/{take}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<WebNovelDTO>>> GetHotWebNovels(int take)
    {
        if (take < 1)
        {
            return BadRequest("Invalid Take Value, Cannot Be Less Than 1");
        }

        var webNovels = await _webNovelRepository.GetByUpdatedAtAsync(DateTime.Now, take);

        if (webNovels == null || !webNovels.Any())
        {
            return NotFound("No WebNovels Found");
        }
        return Ok(webNovels.ToDTO());
    }

    [HttpGet("web-novels/poster/{take}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<WebNovelDTO>>> GetPosterWebNovels(int take)
    {
        if (take < 1)
        {
            return BadRequest("Invalid Take Value, Cannot Be Less Than 1");
        }
        var webNovels = await _webNovelRepository.GetByRandomAsync(take);

        if (webNovels == null || !webNovels.Any())
        {
            return NotFound("No WebNovels Found");
        }

        return Ok(webNovels.ToDTO());
    }

    [HttpGet("web-novels/suggested/{take}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<WebNovelDTO>>> GetRecommendedWebNovels(int take)
    {
        if (take < 1)
        {
            return BadRequest("Invalid Take Value, Cannot Be Less Than 1");
        }

        var webNovels = await _webNovelRepository.GetRandomAsync(take);

        if (webNovels == null || !webNovels.Any())
        {
            return NotFound("No WebNovel Found");
        }

        return Ok(webNovels.ToDTO());
    }


    [HttpGet("web-novels/{id}/manga")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<MangaDTO>> GetWebNovelManga(Guid id)
    {
        var webNovel = await _webNovelRepository.GetByIdAsync(id);

        if (webNovel == null)
        {
            return NotFound($"No WebNovel With Id: {id} Found");
        }

        var manga = webNovel.Manga;

        if (manga == null)
        {
            return NoContent();
        }

        return Ok(webNovel.ToDTO());

    }


    [HttpDelete("web-novels/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteWebNovel(Guid id)
    {
        var webNovel = await _webNovelRepository.GetByIdAsync(id);
        if (webNovel == null)
        {
            return NotFound($"No WebNovel With Id: {id} Found");
        }

        await _webNovelRepository.DeleteAsync(webNovel);
        await _unitOfWork.CommitAsync();
        return Accepted();
    }


    [HttpPost("web-novels")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(WebNovelDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelDTO>> CreateWebNovel([FromBody] WebNovelCreateDTO webNovel)
    {
        var existingWebNovel = await _webNovelRepository.GetBySlugAsync(webNovel.Title.ToSlug());

        if (existingWebNovel != null)
        {
            return Ok(existingWebNovel.ToDTO());
        }

        try
        {

            var authors = new List<AuthorEntity>();
            foreach (var authorId in webNovel.Authors)
            {

                var author = await _authorRepository.GetByIdAsync(authorId);
                if (author == null)
                {
                    return NotFound($"No Author With Id: {authorId} Found");
                }
                authors.Add(author);
            }

            var status = await _statusRepository.GetByIdAsync(webNovel.Status);

            if (status == null)
            {
                return NotFound($"No Status With Id: {webNovel.Status} Found");
            }

            var image = await _imageRepository.GetByIdAsync(webNovel.CoverImage);

            if (image == null)
            {
                return NotFound($"No Image With Id: {webNovel.CoverImage} Found");
            }


            List<GenreEntity> genres = new();

            foreach (var genreId in webNovel.Genres)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);

                if (genre == null)
                {
                    return NotFound($"No Genre With Id: {genreId} Found");
                }
                genres.Add(genre);
            }



            WebNovelEntity newWebNovel = WebNovelEntity.Create(
                webNovel.Title,
                webNovel.Description,
                image,
                authors.ToArray(),
                genres,
                status,
                webNovel.Title.ToSlug()
                );


            if (webNovel.Manga != null)
            {
                var manga = await _mangaRepository.GetByIdAsync(webNovel.Manga.Value);

                if (manga == null)
                {
                    return NotFound($"No Manga With Id: {webNovel.Manga} Found");
                }

                newWebNovel.SetManga(manga);
                _mangaRepository.MarkAsModified(manga);

            }

            await _webNovelRepository.AddAsync(newWebNovel);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetWebNovel), new { id = newWebNovel.Id }, newWebNovel.ToDTO());
        }
        catch (DuplicateEntityException ex)
        {
            existingWebNovel = await _webNovelRepository.GetBySlugAsync(webNovel.Title.ToSlug());

            if (existingWebNovel == null)
            {
                return BadRequest(ex.Message);
            }
            return Ok(existingWebNovel.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("web-novels/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WebNovelDTO>> UpdateWebNovel(Guid id, [FromBody] WebNovelUpdateDTO webNovel)
    {
        var webNovelToUpdate = await _webNovelRepository.GetByIdAsync(id);

        if (webNovelToUpdate == null)
        {
            return NotFound($"No WebNovel With Id: {id} Found");
        }

        try
        {
            if (webNovel.Title != null)
            {
                webNovelToUpdate.SetTitle(webNovel.Title);
            }
            if (webNovel.Description != null)
            {
                webNovelToUpdate.SetDescription(webNovel.Description);
            }
            if (webNovel.CoverImage != null)
            {
                var image = await _imageRepository.GetByIdAsync(webNovel.CoverImage.Value);

                if (image == null)
                {
                    return NotFound($"No Image With Url: {webNovel.CoverImage} Found");
                }

                webNovelToUpdate.SetCoverImage(image);
            }

            if (webNovel.Genres != null)
            {
                if (webNovel.Genres.Count() < 1)
                {
                    return BadRequest("Genres Cannot Be Empty");
                }

                foreach (var genreId in webNovel.Genres)
                {
                    var genre = await _genreRepository.GetByIdAsync(genreId);

                    if (genre == null)
                    {
                        return NotFound($"No Genre With Id: {genreId} Found");
                    }

                    webNovelToUpdate.AddGenre(genre);
                }
            }

            if (webNovel.Status != null)
            {
                var status = await _statusRepository.GetByIdAsync(webNovel.Status.Value);

                if (status == null)
                {
                    return NotFound($"No Status With Id: {webNovel.Status} Found");
                }
                webNovelToUpdate.SetStatus(status);
            }




            if (webNovel.Authors != null)
            {
                foreach (var authorId in webNovel.Authors)
                {

                    var author = await _authorRepository.GetByIdAsync(authorId);

                    if (author == null)
                    {
                        return NotFound($"No Author With Id: {authorId} Found");
                    }

                    webNovelToUpdate.AddAuthor(author);
                }
            }

            if (webNovel.Manga != null)
            {
                var manga = await _mangaRepository.GetByIdAsync(webNovel.Manga.Value);
                if (manga == null)
                {
                    return NotFound($"No Manga With Id: {webNovel.Manga.Value} Found");
                }
                webNovelToUpdate.SetManga(manga);
                _mangaRepository.MarkAsModified(manga);
            }

            _webNovelRepository.MarkAsModified(webNovelToUpdate);
            await _unitOfWork.CommitAsync();

            return Ok(webNovelToUpdate.ToDTO());

        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return BadRequest(e.Message);
        }
    }


    [HttpPost("web-novels/search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelDTO>> SearchWebNovels([FromBody] WebNovelSearchDTO webNovel)
    {
        if (string.IsNullOrWhiteSpace(webNovel.Title))
        {
            return BadRequest($"Invalid Parameter {nameof(webNovel.Title)} Cannot Be Empty or Null");
        }

        var webNovels = await _webNovelRepository.SearchByTitleAsync(webNovel.Title);

        if (webNovels == null || !webNovels.Any())
        {
            return NotFound($"No WebNovel With The Given Criteria Found");
        }
        return Ok(webNovels.ToDTO());
    }

    [HttpPost("web-novels/filtered")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelDTO>> GetFilteredWebNovels([FromBody] WebNovelFilterDTO webNovel)
    {

        if (webNovel.Genres == null && webNovel.Statuses == null)
        {
            return BadRequest("No Filter Criteria Given");
        }


        var webNovels = await _webNovelRepository.GetFilteredAsync(webNovel.Genres, webNovel.Statuses);

        if (webNovels == null || !webNovels.Any())
        {
            return NotFound($"No WebNovel With The Given Criteria Found");
        }

        return Ok(webNovels.ToDTO());
    }


    [HttpPost("web-novels/filtered/page/{page}/size/{pageSize}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<WebNovelDTO>>> GetFilteredWebNovelsPaginated([FromBody] WebNovelFilterDTO webNovel, int page, int pageSize)
    {
        if (webNovel.Genres == null && webNovel.Statuses == null)
        {
            return BadRequest("No Filter Criteria Given");
        }

        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }


        var webNovels = await _webNovelRepository.GetFilteredPagedAsync(webNovel.Genres, webNovel.Statuses, page, pageSize);

        if (webNovels == null || !webNovels.Any())
        {
            return NotFound($"No WebNovel With The Given Criteria Found");
        }

        return Ok(webNovels.ToDTO());
    }


}