using MangaRead.Application.DTOs.Readables.Manga;
using MangaRead.Application.DTOs.Readables.Manga.Type;
using MangaRead.Application.Repositories.Readables.Manga;
using MangaRead.Application.Repositories.Readables.Manga.Type;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Readables.Manga.Type;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace MangaRead.API.Controllers.Readables.Manga.Type;

[Route("/api/v1/")]
[ApiController]
public class MangaTypeController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMangaTypeRepository _mangaTypeRepository;
    private readonly IMangaRepository _mangaRepository;


    public MangaTypeController(IUnitOfWork unitOfWork, IMangaTypeRepository mangaTypeRepository, IMangaRepository mangaRepository)
    {
        _unitOfWork = unitOfWork;
        _mangaTypeRepository = mangaTypeRepository;
        _mangaRepository = mangaRepository;

    }


    [HttpGet("manga-types")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaTypeDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MangaTypeDTO>>> GetMangaTypes()
    {
        var types = await _mangaTypeRepository.GetAllAsync();
        if (types == null || !types.Any())
        {
            return NotFound("No Manga Types Found");
        }
        return Ok(types.ToDTO());
    }

    [HttpGet("manga-types/page/{page:int}/size/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaTypeDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MangaTypeDTO>>> GetMangaTypesPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }

        var mangaTypes = await _mangaTypeRepository.GetAllPagedAsync(page, pageSize);

        if (mangaTypes == null || !mangaTypes.Any())
        {
            return NotFound("No Manga Types Found");
        }
        return Ok(mangaTypes.ToDTO());
    }

    [HttpGet("manga-types/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaTypeDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MangaTypeDTO>> GetMangaTypeById(Guid id)
    {
        var mangaType = await _mangaTypeRepository.GetByIdAsync(id);
        if (mangaType == null)
        {
            return NotFound($"No Manga Type With Id: {id} Found");
        }
        return Ok(mangaType.ToDTO());
    }

    [HttpGet("manga-types/named/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaTypeDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MangaTypeDTO>> GetMangaTypeByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Invalid Name Parameter");
        }

        var mangaType = await _mangaTypeRepository.GetByNameAsync(name);

        if (mangaType == null)
        {
            return NotFound($"No Manga Type With Name: {name} Found");
        }
        return Ok(mangaType.ToDTO());
    }


    [HttpGet("manga-types/slugged/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaTypeDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MangaTypeDTO>> GetMangaTypeBySlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest("Invalid Slug Parameter");
        }

        var type = await _mangaTypeRepository.GetBySlugAsync(slug);
        if (type == null)
        {
            return NotFound($"No Manga Type With Slug: {slug} Found");
        }

        return Ok(type.ToDTO());
    }



    [HttpPost("manga-types")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MangaTypeDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MangaTypeDTO>> CreateMangaType([FromBody] MangaTypeCreateDTO type)
    {
        if (string.IsNullOrEmpty(type.Name))
        {
            return BadRequest($"Invalid Parameter: {nameof(type.Name)} Is Null Or Empty");
        }

        var existingType = await _mangaTypeRepository.GetBySlugAsync(type.Name.ToSlug());
        if (existingType != null)
        {
            return Ok(existingType.ToDTO());
        }

        try
        {
            MangaTypeEntity mangaType = MangaTypeEntity.Create(type.Name, type.Name.ToSlug());

            await _mangaTypeRepository.AddAsync(mangaType);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetMangaTypeById), new { id = mangaType.Id }, mangaType.ToDTO());
        }
        catch (DuplicateEntityException ex)
        {
            Log.Error(ex, $"Tried to create a manga type with name: {type.Name} that already exists");
            existingType = await _mangaTypeRepository.GetBySlugAsync(type.Name.ToSlug());
            if (existingType == null)
            {
                return BadRequest(ex.Message);
            }
            return Ok(existingType.ToDTO());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("manga-types/search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaTypeDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MangaTypeDTO>>> SearchMangaTypeByName([FromBody] MangaTypeSearchDTO type)
    {
        if (string.IsNullOrEmpty(type.Name))
        {
            return BadRequest($"Invalid Parameter: {nameof(type.Name)} Is Null Or Empty");
        }

        var mangaTypes = await _mangaTypeRepository.SearchByNameAsync(type.Name);
        if (mangaTypes == null || !mangaTypes.Any())
        {
            return NotFound("No Manga Types With The Given Criteria Found");
        }
        return Ok(mangaTypes.ToDTO());
    }

    [HttpDelete("manga-types/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteMangaType(Guid id)
    {
        var mangaType = await _mangaTypeRepository.GetByIdAsync(id);
        if (mangaType == null)
        {
            return NotFound($"No Manga Type With Id: {id} Found");
        }
        await _mangaTypeRepository.DeleteAsync(mangaType);
        await _unitOfWork.CommitAsync();
        return Accepted();
    }


    [HttpPut("manga-types/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MangaTypeDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MangaTypeDTO>> UpdateMangaType(Guid id, [FromBody] MangaTypeUpdateDTO type)
    {
        if (string.IsNullOrEmpty(type.Name))
        {
            return BadRequest($"Invalid Parameter: {nameof(type.Name)} Is Null Or Empty");
        }

        var mangaType = await _mangaTypeRepository.GetByIdAsync(id);

        if (mangaType == null)
        {
            return NotFound($"No Manga Type With Id: {id} Found");
        }

        try
        {
            mangaType.SetName(type.Name);
            mangaType.SetSlug(type.Name.ToSlug());

            _mangaTypeRepository.MarkAsModified(mangaType);
            await _unitOfWork.CommitAsync();

            return Ok(mangaType.ToDTO());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("manga-types/{id:guid}/mangas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<MangaDTO>>> GetMangaTypeMangas(Guid id)
    {

        var mangas = await _mangaRepository.GetByTypeAsync(id);

        if (mangas == null || !mangas.Any())
        {
            return NoContent();
        }

        return Ok(mangas.ToDTO());
    }

}