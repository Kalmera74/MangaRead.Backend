using MangaRead.Application.DTOs.Readables.Manga;
using MangaRead.Application.DTOs.Readables.WebNovel;
using MangaRead.Application.DTOs.Status;
using MangaRead.Application.Repositories.Readables.Manga;
using MangaRead.Application.Repositories.Readables.WebNovel;
using MangaRead.Application.Repositories.Status;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Status;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace MangaRead.API.Controllers.Status;

[Route("/api/v1/")]
[ApiController]
public class StatusController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStatusRepository _statusRepository;
    private readonly IMangaRepository _mangaRepository;
    private readonly IWebNovelRepository _webNovelRepository;
    public StatusController(IUnitOfWork unitOfWork, IStatusRepository statusRepository, IMangaRepository mangaRepository, IWebNovelRepository webNovelRepository)
    {
        _unitOfWork = unitOfWork;
        _statusRepository = statusRepository;
        _mangaRepository = mangaRepository;
        _webNovelRepository = webNovelRepository;
    }

    [HttpGet("statuses")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StatusDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<StatusDTO>>> GetStatuses()
    {
        var statuses = await _statusRepository.GetAllAsync();
        if (statuses == null || !statuses.Any())
        {
            return NotFound("No Manga Statuses Found");
        }
        return Ok(statuses.ToDTO());
    }

    [HttpGet("statuses/page/{page:int}/size/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StatusDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<StatusDTO>>> GetStatusesPaged(int page, int pageSize)
    {

        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }

        var statuses = await _statusRepository.GetAllPagedAsync(page, pageSize);

        if (statuses == null || !statuses.Any())
        {
            return NotFound("No Manga Statuses Found");
        }
        return Ok(statuses.ToDTO());
    }

    [HttpGet("statuses/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StatusDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatusDTO>> GetStatus(Guid id)
    {
        StatusEntity? status = await _statusRepository.GetByIdAsync(id);
        if (status == null)
        {
            return NotFound($"No Status With Id: {id} Found");
        }
        return status.ToDTO();
    }

    [HttpDelete("statuses/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteStatus(Guid id)
    {
        var status = await _statusRepository.GetByIdAsync(id);
        if (status == null)
        {
            return NotFound($"No Status With Id: {id} Found");
        }
        await _statusRepository.DeleteAsync(status);
        await _unitOfWork.CommitAsync();
        return Accepted();
    }

    [HttpGet("statuses/named/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StatusDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatusDTO>> GetStatusByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Invalid Name Parameter");
        }

        var status = await _statusRepository.GetByNameAsync(name);

        if (status == null)
        {
            return NotFound($"No Status With Name: {name} Found");
        }

        return status.ToDTO();

    }


    [HttpGet("statuses/slugged/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StatusDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StatusDTO>> GetStatusBySlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest("Invalid Slug Parameter");
        }

        var status = await _statusRepository.GetBySlugAsync(slug);
        if (status == null)
        {
            return NotFound($"No Status With Slug: {slug} Found");
        }

        return Ok(status.ToDTO());
    }



    [HttpGet("statuses/{id:guid}/mangas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<MangaDTO>>> GetStatusMangas(Guid id)
    {
        var status = await _statusRepository.GetByIdAsync(id);

        if (status == null)
        {
            return NotFound($"No Status With Id: {id} Found");
        }

        var mangas = await _mangaRepository.GetByStatusAsync(id);

        if (mangas == null || !mangas.Any())
        {
            return NoContent();
        }

        return Ok(mangas.ToDTO());
    }


    [HttpGet("statuses/{id:guid}/web-novels")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<MangaDTO>>> GetStatusWebNovels(Guid id)
    {
        var status = await _statusRepository.GetByIdAsync(id);

        if (status == null)
        {
            return NotFound($"No Status With Id: {id} Found");
        }

        var webNovels = await _webNovelRepository.GetByStatusAsync(id);

        if (webNovels == null || !webNovels.Any())
        {
            return NoContent();
        }

        return Ok(webNovels.ToDTO());
    }


    [HttpPost("statuses/search")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StatusDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatusDTO>> SearchStatusByName([FromBody] StatusSearchDTO status)
    {
        if (string.IsNullOrEmpty(status.Name))
        {
            return BadRequest($"Invalid Parameter: {nameof(status.Name)} Is Null Or Empty");
        }

        var statuses = await _statusRepository.SearchByNameAsync(status.Name);
        if (statuses == null)
        {
            return NotFound("No Statuses With The Given Criteria Found");
        }
        return Ok(statuses.ToDTO());
    }

    [HttpPost("statuses")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StatusDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StatusDTO>> CreateStatus([FromBody] StatusCreateDTO status)
    {
        var existingStatus = await _statusRepository.GetBySlugAsync(status.Name.ToSlug());
        if (existingStatus != null)
        {
            return Ok(existingStatus.ToDTO());
        }

        try
        {
            StatusEntity newStatus = StatusEntity.Create(status.Name, status.Name.ToSlug());

            await _statusRepository.AddAsync(newStatus);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetStatus), new { id = newStatus.Id }, newStatus.ToDTO());
        }
        catch (DuplicateEntityException ex)
        {
            Log.Error(ex, $"Tried to create a status with name: {status.Name} that already exists");
            existingStatus = await _statusRepository.GetBySlugAsync(status.Name.ToSlug());
            if (existingStatus == null)
            {
                return BadRequest(ex.Message);
            }
            return Ok(existingStatus.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("statuses/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StatusDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatusDTO>> UpdateStatus(Guid id, [FromBody] StatusUpdateDTO status)
    {
        var existingStatus = await _statusRepository.GetByIdAsync(id);
        if (existingStatus == null)
        {
            return NotFound($"No Manga Status With Id: {id} Found");
        }

        try
        {
            if (status.Name != null)
            {

                existingStatus.SetName(status.Name);
                existingStatus.SetSlug(status.Name.ToSlug());
            }

            _statusRepository.MarkAsModified(existingStatus);
            await _unitOfWork.CommitAsync();

            return Ok(existingStatus.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }
    }
}