using MangaRead.Application.DTOs.Readables.WebNovel.Chapter.Content;
using MangaRead.Application.Repositories.Readables.WebNovel.Chapter;
using MangaRead.Application.Repositories.Readables.WebNovel.Chapter.Content;
using MangaRead.Application.Repositories.System.Image;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Readables.WebNovel.Chapter.Content;
using Microsoft.AspNetCore.Mvc;

namespace MangaRead.API.Controllers.Readables.WebNovel.Chapter.Content;

[Route("/api/v1/")]
[ApiController]
public class WebNovelChapterContentController : ControllerBase
{

    private readonly IWebNovelChapterContentRepository _webNovelChapterContentRepository;
    private readonly IWebNovelChapterRepository _webNovelChapterRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;
    public WebNovelChapterContentController(IUnitOfWork unitOfWork, IWebNovelChapterContentRepository webNovelChapterContentRepository, IWebNovelChapterRepository webNovelChapterRepository, IImageRepository imageRepository)
    {
        _unitOfWork = unitOfWork;
        _webNovelChapterContentRepository = webNovelChapterContentRepository;
        _webNovelChapterRepository = webNovelChapterRepository;
        _imageRepository = imageRepository;
    }


    [HttpGet("web-novel-chapter-contents")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelChapterContentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<WebNovelChapterContentDTO>>> GetAllWebNovelChapterContents()
    {
        var mangaChapterContents = await _webNovelChapterContentRepository.GetAllAsync();

        if (mangaChapterContents == null || !mangaChapterContents.Any())
        {
            return NotFound("No Manga Chapter Contents Found");
        }

        return Ok(mangaChapterContents.ToDTO());
    }

    [HttpGet("web-novel-chapter-contents/page/{page:int}/size/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelChapterContentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<WebNovelChapterContentDTO>>> GetAllWebNovelChapterContentsPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }

        var mangaChapterContents = await _webNovelChapterContentRepository.GetAllPagedAsync(page, pageSize);

        if (mangaChapterContents == null || !mangaChapterContents.Any())
        {
            return NotFound("No Manga Chapter Contents Found");
        }

        return Ok(mangaChapterContents.ToDTO());
    }


    [HttpGet("web-novel-chapter-contents/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelChapterContentDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WebNovelChapterContentDTO>> GetWebNovelChapterContentById(Guid id)
    {
        var mangaChapterContent = await _webNovelChapterContentRepository.GetByIdAsync(id);

        if (mangaChapterContent == null)
        {
            return NotFound($"No Manga Chapter Content Found With Id: {id}");
        }
        return Ok(mangaChapterContent.ToDTO());
    }

    [HttpPost("web-novel-chapter-contents")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(WebNovelChapterContentDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelChapterContentDTO>> CreateWebNovelChapterContent([FromBody] WebNovelChapterContentCreateDTO webNovelChapterContent)
    {

        var chapter = await _webNovelChapterRepository.GetByIdAsync(webNovelChapterContent.ChapterId);

        if (chapter == null)
        {
            return NotFound($"No Chapter With Id: {webNovelChapterContent.ChapterId} Found");
        }

        var existingChapterContent = await _webNovelChapterContentRepository.GetByChapterAsync(webNovelChapterContent.ChapterId);

        if (existingChapterContent != null)
        {
            return Ok(existingChapterContent.ToDTO());
        }

        try
        {
            WebNovelChapterContentEntity newMangaChapterContent;


            newMangaChapterContent = WebNovelChapterContentEntity.Create(
                webNovelChapterContent.Title,
                webNovelChapterContent.Body,
                chapter
                );

            _webNovelChapterRepository.MarkAsModified(chapter);

            if (webNovelChapterContent.MetaTitle != null)
            {
                newMangaChapterContent.SetMetaTitle(webNovelChapterContent.MetaTitle);
            }
            if (webNovelChapterContent.MetaDescription != null)
            {
                newMangaChapterContent.SetMetaDescription(webNovelChapterContent.MetaDescription);
            }



            chapter.SetContent(newMangaChapterContent);

            await _webNovelChapterContentRepository.AddAsync(newMangaChapterContent);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetWebNovelChapterContentById), new { id = newMangaChapterContent.Id }, newMangaChapterContent.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }

    }


    [HttpPut("web-novel-chapter-contents/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelChapterContentDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelChapterContentDTO>> UpdateWebNovelChapterContent(Guid id, [FromBody] WebNovelChapterContentUpdateDTO webNovelChapterContent)
    {
        var webNovelChapterContentToUpdate = await _webNovelChapterContentRepository.GetByIdAsync(id);

        if (webNovelChapterContentToUpdate == null)
        {
            return NotFound($"No Chapter Content With Id: {id} Found");
        }

        try
        {
            if (webNovelChapterContent.MetaTitle != null)
            {
                webNovelChapterContentToUpdate.SetMetaTitle(webNovelChapterContent.MetaTitle);
            }
            if (webNovelChapterContent.MetaDescription != null)
            {
                webNovelChapterContentToUpdate.SetMetaDescription(webNovelChapterContent.MetaDescription);
            }

            if (webNovelChapterContent.Title != null)
            {
                webNovelChapterContentToUpdate.SetTitle(webNovelChapterContent.Title);
            }

            if (webNovelChapterContent.Body != null)
            {
                webNovelChapterContentToUpdate.SetContentBody(webNovelChapterContent.Body);
            }

            if (webNovelChapterContent.ChapterId != null)
            {
                var chapter = await _webNovelChapterRepository.GetByIdAsync(webNovelChapterContent.ChapterId.Value);
                if (chapter == null)
                {
                    return NotFound($"No Chapter With Id: {webNovelChapterContent.ChapterId} Found");
                }

                webNovelChapterContentToUpdate.SetChapter(chapter);
                _webNovelChapterRepository.MarkAsModified(chapter);
            }

            _webNovelChapterContentRepository.MarkAsModified(webNovelChapterContentToUpdate);
            await _unitOfWork.CommitAsync();

            return Ok(webNovelChapterContentToUpdate.ToDTO());
        }

        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete("web-novel-chapter-contents/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteWebNovelChapterContent(Guid id)
    {
        var mangaChapterContent = await _webNovelChapterContentRepository.GetByIdAsync(id);

        if (mangaChapterContent == null)
        {
            return NotFound($"No Manga Chapter Content Found With Id: {id}");
        }

        await _webNovelChapterContentRepository.DeleteAsync(mangaChapterContent);
        await _unitOfWork.CommitAsync();

        return Accepted();
    }
}