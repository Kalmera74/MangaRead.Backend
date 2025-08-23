using MangaRead.Application.DTOs.Readables.Manga.Chapter;
using MangaRead.Application.DTOs.Readables.Manga.Chapter.Content;
using MangaRead.Application.Repositories.Readables.Manga;
using MangaRead.Application.Repositories.Readables.Manga.Chapter;
using MangaRead.Application.Repositories.Readables.Manga.Chapter.Content;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Readables.Manga;
using MangaRead.Domain.Entities.Readables.Manga.Chapter;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MangaRead.API.Controllers.Readables.Manga.Chapter;

[Route("/api/v1/")]
[ApiController]
public class MangaChapterController : ControllerBase
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMangaRepository _mangaRepository;
    private readonly IMangaChapterRepository _mangaChapterRepository;
    private readonly IMangaChapterContentRepository _mangaChapterContentRepository;

    public MangaChapterController(IUnitOfWork unitOfWork, IMangaChapterRepository mangaChapterRepository, IMangaChapterContentRepository mangaChapterContentRepository, IMangaRepository mangaRepository)
    {
        _unitOfWork = unitOfWork;
        _mangaRepository = mangaRepository;
        _mangaChapterRepository = mangaChapterRepository;
        _mangaChapterContentRepository = mangaChapterContentRepository;
    }

    [HttpGet("manga-chapters")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SimpleMangaChapterDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<SimpleMangaChapterDTO>>> GetAllChapters()
    {
        var chapters = await _mangaChapterRepository.GetAllAsync();
        if (chapters == null || !chapters.Any())
        {
            return NotFound("No Chapters Found");
        }
        return Ok(chapters.ToSimpleDTO());

    }

    [HttpGet("manga-chapters/page/{page:int}/size/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SimpleMangaChapterDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<SimpleMangaChapterDTO>>> GetAllChaptersPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }
        var chapters = await _mangaChapterRepository.GetAllPagedAsync(page, pageSize);
        if (chapters == null || !chapters.Any())
        {
            return NotFound("No Chapters Found");
        }
        return Ok(chapters.ToSimpleDTO());
    }

    [HttpGet("manga-chapters/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaChapterDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MangaChapterDTO>> GetChapterById(Guid id)
    {
        var chapter = await _mangaChapterRepository.GetByIdAsync(id);
        if (chapter == null)
        {
            return NotFound($"No Chapter Found with Id: {id}");
        }
        return Ok(chapter.ToDTO());
    }
    [HttpGet("manga-chapters/slugged/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaChapterDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MangaChapterDTO>> GetMangaChapterBySlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest("Invalid Slug Parameter");
        }

        var chapter = await _mangaChapterRepository.GetBySlugAsync(slug);
        if (chapter == null)
        {
            return NotFound($"No Chapter With Slug: {slug} Found");
        }

        return Ok(chapter.ToDTO());
    }

    [HttpGet("mangas/{id:guid}/chapters")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaChapterDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<MangaChapterDTO>> GetMangaChapter(Guid id)
    {
        var manga = await _mangaRepository.GetByIdAsync(id);
        if (manga == null)
        {
            return NotFound($"No Manga With Id: {id} Found");
        }

        var chapters = manga.Chapters;
        chapters = chapters.OrderBy(x => x.CreatedAt).ToList();

        if (chapters == null || !chapters.Any())
        {
            return NoContent();
        }
        return Ok(chapters.ToDTO());
    }

    [HttpGet("mangas/{id:guid}/latest/chapter")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaChapterDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<MangaChapterDTO>> GetLatestMangaChapter(Guid id)
    {
        var manga = await _mangaRepository.GetByIdAsync(id);
        if (manga == null)
        {
            return NotFound($"No Manga With Id: {id} Found");
        }

        var chapters = manga.Chapters;

        if (chapters == null || !chapters.Any())
        {
            return NoContent();
        }
        return Ok(chapters.Last().ToDTO());
    }

    [HttpGet("manga-chapters/{id:guid}/content")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaChapterContentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MangaChapterContentDTO>>> GetChapterContents(Guid id)
    {
        var chapter = await _mangaChapterRepository.GetByIdAsync(id);


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

            chapter.Manga.IncreaseViewCountByOne();
            _mangaRepository.MarkAsModified(chapter.Manga);

            await _unitOfWork.CommitAsync();
            var content = chapter.Content.OrderBy(x => x.Order).ToList();
            return Ok(content.ToDTO());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("manga-chapters")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MangaChapterDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MangaChapterDTO>> CreateChapter([FromBody] MangaChapterCreateDTO chapter)
    {

        var manga = await _mangaRepository.GetByIdAsync(chapter.MangaId);

        if (manga == null)
        {
            return NotFound($"No Manga With Id: {chapter.MangaId} Found");
        }

        var existingChapter = manga.Chapters.FirstOrDefault(c => c.Title == chapter.Title);
        if (existingChapter != null)
        {
            return Ok(existingChapter.ToDTO());
        }


        try
        {
            MangaChapterEntity newChapter = MangaChapterEntity.Create
            (
                manga,
                chapter.Title,
                chapter.Title.ToSlug()
            );

            if (!string.IsNullOrEmpty(chapter.MetaTitle))
            {

                newChapter.SetMetaTitle(chapter.MetaTitle);
            }
            if (!string.IsNullOrEmpty(chapter.MetaDescription))
            {

                newChapter.SetMetaTitle(chapter.MetaDescription);
            }


            var createdMangaChapter = await _mangaChapterRepository.AddAsync(newChapter);

            LinkChapters(manga, createdMangaChapter);

            _mangaRepository.MarkAsModified(manga);

            await _unitOfWork.CommitAsync();


            return CreatedAtAction(nameof(GetChapterById), new { id = createdMangaChapter.Id }, createdMangaChapter.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }

    }

    private bool LinkChapters(MangaEntity manga, MangaChapterEntity chapter)
    {
        try
        {
            var currentChapterIndex = manga.Chapters.IndexOf(chapter);

            if (currentChapterIndex == -1)
            {
                Log.Error($"Unexpected error manga with id: {manga.Id} does not contain chapter with id: {chapter.Id}");
                return false;
            }



            if (manga.Chapters.Count == 1)
            {
                return true;
            }


            var previousChapter = manga.Chapters[currentChapterIndex - 1];

            previousChapter.SetNextChapter(chapter);
            _mangaChapterRepository.MarkAsModified(previousChapter);


            return true;
        }
        catch (Exception ex)
        {
            Log.Error($"Unexpected error while linking chapters of manga {manga.Id}, Reason: {ex.Message}");
            return false;
        }

    }

    [HttpDelete("manga-chapters/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteChapter(Guid id)
    {
        var chapter = await _mangaChapterRepository.GetByIdAsync(id);

        if (chapter == null)
        {
            return NotFound($"No Chapter Found With Id: {id}");
        }

        if (!UnlinkChapters(chapter))
        {
            Log.Warning($"Unexpected error while unlinking chapters from {chapter.Id} of manga {chapter.Manga.Id}, chapter will be deleted but the links will not be removed");
        }

        await _mangaChapterRepository.DeleteAsync(chapter);
        await _unitOfWork.CommitAsync();



        return Accepted();
    }

    private bool UnlinkChapters(MangaChapterEntity chapter)
    {
        try
        {

            chapter.RemovePreviousChapter();
            chapter.RemoveNextChapter(); ;

            Log.Information($"Unlinked previous and next chapters of chapter {chapter.Id} of manga {chapter.Manga.Id} successfully");

            return true;
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            Log.Error($"Unexpected error while unlinking chapters of chapter {chapter.Id} of manga {chapter.Manga.Id}, Reason: {ex.Message}");
            return false;
        }
    }




    [HttpPut("manga-chapters/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaChapterDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MangaChapterDTO>> UpdateChapter(Guid id, [FromBody] MangaChapterUpdateDTO chapter)
    {
        MangaChapterEntity? chapterToUpdate = await _mangaChapterRepository.GetByIdAsync(id);
        if (chapterToUpdate == null)
        {
            return NotFound($"No Chapter With Id: {id} Found");
        }

        try
        {
            if (chapter.Title != null)
            {
                chapterToUpdate.SetTitle(chapter.Title);
                chapterToUpdate.SetSlug(chapter.Title.ToSlug());
            }

            if (chapter.MangaId != null)
            {
                var manga = await _mangaRepository.GetByIdAsync(chapter.MangaId.Value);

                if (manga == null)
                {
                    return NotFound($"No Manga With Id: {chapter.MangaId} Found");
                }

                chapterToUpdate.SetManga(manga);
                _mangaRepository.MarkAsModified(manga);
            }

            if (chapter.PreviousChapterId != null)
            {
                var previousChapter = await _mangaChapterRepository.GetByIdAsync(chapter.PreviousChapterId.Value);

                if (previousChapter == null)
                {
                    return NotFound($"No Chapter With Id: {chapter.PreviousChapterId} Found For Previous Chapter");
                }

                chapterToUpdate.SetPreviousChapter(previousChapter);
                _mangaChapterRepository.MarkAsModified(previousChapter);
            }

            if (chapter.NextChapterId != null)
            {
                var nextChapter = await _mangaChapterRepository.GetByIdAsync(chapter.NextChapterId.Value);
                if (nextChapter == null)
                {
                    return NotFound($"No Chapter With Id: {chapter.NextChapterId} Found For Next Chapter");
                }

                chapterToUpdate.SetNextChapter(nextChapter);
                _mangaChapterRepository.MarkAsModified(nextChapter);
            }

            if (chapter.Content != null)
            {

                foreach (var content in chapterToUpdate.Content)
                {
                    await _mangaChapterContentRepository.DeleteAsync(content);
                }

                chapterToUpdate.ClearContent();

                foreach (var content in chapter.Content)
                {
                    var existingContent = await _mangaChapterContentRepository.GetByIdAsync(content.Id);
                    if (existingContent == null)
                    {
                        return NotFound($"No Chapter Content With Id: {content.Id} Found");
                    }

                    chapterToUpdate.AddContent(existingContent);
                    _mangaChapterContentRepository.MarkAsModified(existingContent);
                }
            }

            _mangaChapterRepository.MarkAsModified(chapterToUpdate);
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