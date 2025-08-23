using MangaRead.Application.DTOs.Readables.WebNovel.Chapter;
using MangaRead.Application.DTOs.Readables.WebNovel.Chapter.Content;
using MangaRead.Application.Repositories.Readables.WebNovel;
using MangaRead.Application.Repositories.Readables.WebNovel.Chapter;
using MangaRead.Application.Repositories.Readables.WebNovel.Chapter.Content;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Readables.WebNovel.Chapter;
using Microsoft.AspNetCore.Mvc;

namespace MangaRead.API.Controllers.Readables.WebNovel.Chapter;

[Route("/api/v1/")]
[ApiController]
public class WebNovelChapterController : ControllerBase
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebNovelRepository _webNovelRepository;
    private readonly IWebNovelChapterRepository _webNovelChapterRepository;
    private readonly IWebNovelChapterContentRepository _webNovelChapterContentRepository;

    public WebNovelChapterController(IUnitOfWork unitOfWork, IWebNovelChapterRepository webNovelChapterRepository, IWebNovelChapterContentRepository webNovelChapterContentRepository, IWebNovelRepository webNovelRepository)
    {
        _unitOfWork = unitOfWork;
        _webNovelRepository = webNovelRepository;
        _webNovelChapterRepository = webNovelChapterRepository;
        _webNovelChapterContentRepository = webNovelChapterContentRepository;
    }

    [HttpGet("web-novel-chapters")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelChapterDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<WebNovelChapterDTO>>> GetAllChapters()
    {
        var chapters = await _webNovelChapterRepository.GetAllAsync();
        if (chapters == null || !chapters.Any())
        {
            return NotFound("No Chapters Found");
        }
        return Ok(chapters.ToDTO());

    }

    [HttpGet("web-novel-chapters/page/{page:int}/size/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelChapterDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<WebNovelChapterDTO>>> GetAllChaptersPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }
        var chapters = await _webNovelChapterRepository.GetAllPagedAsync(page, pageSize);
        if (chapters == null || !chapters.Any())
        {
            return NotFound("No Chapters Found");
        }
        return Ok(chapters.ToDTO());
    }

    [HttpGet("web-novel-chapters/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelChapterDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WebNovelChapterDTO>> GetChapterById(Guid id)
    {
        var chapter = await _webNovelChapterRepository.GetByIdAsync(id);
        if (chapter == null)
        {
            return NotFound($"No Chapter Found with Id: {id}");
        }
        return Ok(chapter.ToDTO());
    }


    [HttpGet("web-novel-chapters/slugged/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelChapterDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelChapterDTO>> GetMangaChapterBySlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest("Invalid Slug Parameter");
        }

        var chapter = await _webNovelChapterRepository.GetBySlugAsync(slug);
        if (chapter == null)
        {
            return NotFound($"No Chapter With Slug: {slug} Found");
        }

        return Ok(chapter.ToDTO());
    }



    [HttpGet("web-novel-chapters/{id:guid}/content")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelChapterContentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<WebNovelChapterContentDTO>>> GetChapterContents(Guid id)
    {
        var chapter = await _webNovelChapterRepository.GetByIdAsync(id);


        if (chapter == null)
        {
            return NotFound($"No Chapter With Id: {id} Found");
        }

        if (chapter.Content == null)
        {
            return NoContent();
        }

        try
        {

            _webNovelRepository.MarkAsModified(chapter.WebNovel);
            await _unitOfWork.CommitAsync();

            return Ok(chapter.Content.ToDTO());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("web-novel-chapters")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(WebNovelChapterDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelChapterDTO>> CreateChapter([FromBody] WebNovelChapterCreateDTO chapter)
    {
        var webNovel = await _webNovelRepository.GetByIdAsync(chapter.WebNovelId);

        if (webNovel == null)
        {
            return NotFound($"No Manga With Id: {chapter.WebNovelId} Found");
        }

        var existingChapter = webNovel.Chapters.FirstOrDefault(c => c.Title == chapter.Title);
        if (existingChapter != null)
        {
            return BadRequest("Chapter Already Exists");
        }

        try
        {
            WebNovelChapterEntity newChapter = WebNovelChapterEntity.Create(
                webNovel,
                chapter.Title,
                chapter.Title.ToSlug()
                );

            _webNovelRepository.MarkAsModified(webNovel);

            await _webNovelChapterRepository.AddAsync(newChapter);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetChapterById), new { id = newChapter.Id }, newChapter.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }

    }

    [HttpDelete("web-novel-chapters/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteChapter(Guid id)
    {
        var chapter = await _webNovelChapterRepository.GetByIdAsync(id);

        if (chapter == null)
        {
            return NotFound($"No Chapter Found With Id: {id}");
        }

        await _webNovelChapterRepository.DeleteAsync(chapter);
        await _unitOfWork.CommitAsync();

        return Accepted();
    }

    [HttpPut("web-novel-chapters/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelChapterDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelChapterDTO>> UpdateChapter(Guid id, [FromBody] WebNovelChapterUpdateDTO chapter)
    {
        WebNovelChapterEntity? chapterToUpdate = await _webNovelChapterRepository.GetByIdAsync(id);

        if (chapterToUpdate == null)
        {
            return NotFound($"No Chapter With Id: {id} Found");
        }

        try
        {
            if (chapter.Title != null)
            {
                chapterToUpdate.SetTitle(chapter.Title);
            }

            if (chapter.WebNovelId != null)
            {
                var webNovel = await _webNovelRepository.GetByIdAsync(chapter.WebNovelId.Value);

                if (webNovel == null)
                {
                    return NotFound($"No Manga With Id: {chapter.WebNovelId} Found");
                }

                chapterToUpdate.SetWebNovel(webNovel);
                _webNovelRepository.MarkAsModified(webNovel);
            }

            if (chapter.PreviousChapterId != null)
            {
                var previousChapter = await _webNovelChapterRepository.GetByIdAsync(chapter.PreviousChapterId.Value);

                if (previousChapter == null)
                {
                    return NotFound($"No Chapter With Id: {chapter.PreviousChapterId} Found For Previous Chapter");
                }

                chapterToUpdate.SetPreviousChapter(previousChapter);
                _webNovelChapterRepository.MarkAsModified(previousChapter);
            }

            if (chapter.NextChapterId != null)
            {
                var nextChapter = await _webNovelChapterRepository.GetByIdAsync(chapter.NextChapterId.Value);
                if (nextChapter == null)
                {
                    return NotFound($"No Chapter With Id: {chapter.NextChapterId} Found For Next Chapter");
                }

                chapterToUpdate.SetNextChapter(nextChapter);
                _webNovelChapterRepository.MarkAsModified(nextChapter);
            }

            if (chapter.ContentId != null)
            {
                var content = await _webNovelChapterContentRepository.GetByChapterAsync(chapter.ContentId.Value);
                if (content == null)
                {
                    return NotFound($"No Chapter Content With Id: {chapter.ContentId} Found");
                }

                chapterToUpdate.SetContent(content);
                _webNovelChapterContentRepository.MarkAsModified(content);
            }
            _webNovelChapterRepository.MarkAsModified(chapterToUpdate);
            await _unitOfWork.CommitAsync();

            return Ok(chapterToUpdate.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }
    }

}