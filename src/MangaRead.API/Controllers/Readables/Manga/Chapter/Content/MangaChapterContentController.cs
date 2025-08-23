using MangaRead.Application.DTOs.Readables.Manga.Chapter.Content;
using MangaRead.Application.Repositories.Readables.Manga.Chapter;
using MangaRead.Application.Repositories.Readables.Manga.Chapter.Content;
using MangaRead.Application.Repositories.System.Image;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Readables.Manga.Chapter.Content;
using Microsoft.AspNetCore.Mvc;

namespace MangaRead.API.Controllers.Readables.Manga.Chapter.Content;

[Route("/api/v1/")]
[ApiController]
public class MangaChapterContentController : ControllerBase
{

    private readonly IMangaChapterContentRepository _mangaChapterContentRepository;
    private readonly IMangaChapterRepository _mangaChapterRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;
    public MangaChapterContentController(IUnitOfWork unitOfWork, IMangaChapterContentRepository mangaChapterContentRepository, IMangaChapterRepository mangaChapterRepository, IImageRepository imageRepository)
    {
        _unitOfWork = unitOfWork;
        _mangaChapterContentRepository = mangaChapterContentRepository;
        _mangaChapterRepository = mangaChapterRepository;
        _imageRepository = imageRepository;
    }


    [HttpGet("manga-chapter-contents")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaChapterContentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MangaChapterContentDTO>>> GetAllMangaChapterContents()
    {
        var mangaChapterContents = await _mangaChapterContentRepository.GetAllAsync();

        if (mangaChapterContents == null || !mangaChapterContents.Any())
        {
            return NotFound("No Manga Chapter Contents Found");
        }

        return Ok(mangaChapterContents.ToDTO());
    }

    [HttpGet("manga-chapter-contents/page/{page:int}/size/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaChapterContentDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MangaChapterContentDTO>>> GetAllMangaChapterContentsPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }

        var mangaChapterContents = await _mangaChapterContentRepository.GetAllPagedAsync(page, pageSize);

        if (mangaChapterContents == null || !mangaChapterContents.Any())
        {
            return NotFound("No Manga Chapter Contents Found");
        }

        return Ok(mangaChapterContents.ToDTO());
    }


    [HttpGet("manga-chapter-contents/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaChapterContentDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MangaChapterContentDTO>> GetMangaChapterContentById(Guid id)
    {
        var mangaChapterContent = await _mangaChapterContentRepository.GetByIdAsync(id);

        if (mangaChapterContent == null)
        {
            return NotFound($"No Manga Chapter Content Found With Id: {id}");
        }
        return Ok(mangaChapterContent.ToDTO());
    }

    [HttpPost("manga-chapter-contents")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MangaChapterContentDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MangaChapterContentDTO>> CreateMangaChapterContent([FromBody] MangaChapterContentCreateDTO mangaChapterContent)
    {
        var chapter = await _mangaChapterRepository.GetByIdAsync(mangaChapterContent.ChapterId);

        if (chapter == null)
        {
            return NotFound($"No Chapter With Id: {mangaChapterContent.ChapterId} Found");
        }

        try
        {
            var image = mangaChapterContent.Image;
            var imageEntity = await _imageRepository.GetByIdAsync(image.Id);

            if (imageEntity == null)
            {
                return NotFound($"No Image With Id: {image.Id} Found");
            }

            var newMangaChapterContent = MangaChapterContentEntity.Create(chapter, imageEntity);

            _mangaChapterRepository.MarkAsModified(chapter);

            await _mangaChapterContentRepository.AddAsync(newMangaChapterContent);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetMangaChapterContentById), new { id = newMangaChapterContent.Id }, newMangaChapterContent.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }


    }


    [HttpPut("manga-chapter-contents/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaChapterContentDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MangaChapterContentDTO>> UpdateMangaChapterContent(Guid id, [FromBody] MangaChapterContentUpdateDTO mangaChapterContent)
    {
        if (mangaChapterContent.ChapterId == null && mangaChapterContent.Image == null && mangaChapterContent.Order == null)
        {
            return BadRequest("No Fields To Update");
        }

        var mangaChapterContentToUpdate = await _mangaChapterContentRepository.GetByIdAsync(id);

        if (mangaChapterContentToUpdate == null)
        {
            return NotFound($"No Manga Chapter Content With Id: {id} Found");
        }

        try
        {

            if (mangaChapterContent.Image != null)
            {
                var image = mangaChapterContent.Image;

                var realImage = await _imageRepository.GetByIdAsync(image.Id);

                if (realImage == null)
                {
                    return NotFound($"No Image With Id: {image.Id} Found");
                }

                mangaChapterContentToUpdate.SetItem(realImage);
            }

            if (mangaChapterContent.Order != null)
            {
                mangaChapterContentToUpdate.SetOrder(mangaChapterContent.Order.Value);
            }

            if (mangaChapterContent.ChapterId != null)
            {
                var chapter = await _mangaChapterRepository.GetByIdAsync(mangaChapterContent.ChapterId.Value);
                if (chapter == null)
                {
                    return NotFound($"No Chapter With Id: {mangaChapterContent.ChapterId} Found");
                }

                mangaChapterContentToUpdate.SetChapter(chapter);
                _mangaChapterRepository.MarkAsModified(chapter);
            }

            _mangaChapterContentRepository.MarkAsModified(mangaChapterContentToUpdate);
            await _unitOfWork.CommitAsync();

            return Ok(mangaChapterContentToUpdate.ToDTO());
        }

        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete("manga-chapter-contents/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteMangaChapterContent(Guid id)
    {
        var mangaChapterContent = await _mangaChapterContentRepository.GetByIdAsync(id);

        if (mangaChapterContent == null)
        {
            return NotFound($"No Manga Chapter Content Found With Id: {id}");
        }

        await _mangaChapterContentRepository.DeleteAsync(mangaChapterContent);
        await _unitOfWork.CommitAsync();

        return Accepted();
    }
}