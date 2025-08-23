using MangaRead.Application.DTOs.Readables.WebNovel;
using MangaRead.Application.DTOs.Readables.WebNovel.Type;
using MangaRead.Application.Repositories.Readables.WebNovel;
using MangaRead.Application.Repositories.Readables.WebNovel.Type;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Readables.WebNovel.Type;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace MangaRead.API.Controllers.Readables.WebNovel.Type;

[Route("/api/v1/")]
[ApiController]
public class WebNovelTypeController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebNovelTypeRepository _webNovelTypeRepository;
    private readonly IWebNovelRepository _webNovelRepository;


    public WebNovelTypeController(IUnitOfWork unitOfWork, IWebNovelTypeRepository webNovelTypeRepository, IWebNovelRepository webNovelRepository)
    {
        _unitOfWork = unitOfWork;
        _webNovelTypeRepository = webNovelTypeRepository;
        _webNovelRepository = webNovelRepository;

    }


    [HttpGet("web-novel-types")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelTypeDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<WebNovelTypeDTO>>> GetWebNovelTypes()
    {
        var types = await _webNovelTypeRepository.GetAllAsync();
        if (types == null || !types.Any())
        {
            return NotFound("No WebNovel Types Found");
        }
        return Ok(types.ToDTO());
    }

    [HttpGet("web-novel-types/page/{page:int}/size/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelTypeDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<WebNovelTypeDTO>>> GetWebNovelTypesPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }

        var webNovelTypes = await _webNovelTypeRepository.GetAllPagedAsync(page, pageSize);

        if (webNovelTypes == null || !webNovelTypes.Any())
        {
            return NotFound("No WebNovel Types Found");
        }
        return Ok(webNovelTypes.ToDTO());
    }

    [HttpGet("web-novel-types/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelTypeDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WebNovelTypeDTO>> GetWebNovelTypeById(Guid id)
    {
        var webNovelType = await _webNovelTypeRepository.GetByIdAsync(id);
        if (webNovelType == null)
        {
            return NotFound($"No WEbNovel Type With Id: {id} Found");
        }
        return Ok(webNovelType.ToDTO());
    }

    [HttpGet("web-novel-types/named/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelTypeDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WebNovelTypeDTO>> GetWebNovelTypeByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Invalid Name Parameter");
        }

        var webNovelType = await _webNovelTypeRepository.GetByNameAsync(name);

        if (webNovelType == null)
        {
            return NotFound($"No WebNovel Type With Name: {name} Found");
        }
        return Ok(webNovelType.ToDTO());
    }

    [HttpGet("web-novel-types/slugged/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelTypeDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelTypeDTO>> GetWebNovelTypeBySlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest("Invalid Slug Parameter");
        }

        var type = await _webNovelTypeRepository.GetBySlugAsync(slug);
        if (type == null)
        {
            return NotFound($"No WebNovel Type With Slug: {slug} Found");
        }

        return Ok(type.ToDTO());
    }



    [HttpPost("web-novel-types")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(WebNovelTypeDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelTypeDTO>> CreateWebNovelType([FromBody] WebNovelTypeCreateDTO type)
    {
        if (string.IsNullOrEmpty(type.Name))
        {
            return BadRequest($"Invalid Parameter: {nameof(type.Name)} Is Null Or Empty");
        }

        var existingType = await _webNovelTypeRepository.GetBySlugAsync(type.Name.ToSlug());
        if (existingType != null)
        {
            return Ok(existingType.ToDTO());
        }

        try
        {
            WebNovelTypeEntity webNovelType = WebNovelTypeEntity.Create(type.Name, type.Name.ToSlug());

            await _webNovelTypeRepository.AddAsync(webNovelType);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetWebNovelTypeById), new { id = webNovelType.Id }, webNovelType.ToDTO());
        }
        catch (DuplicateEntityException ex)
        {
            Log.Error(ex, $"Tried to create a web novel type with name: {type.Name} that already exists");
            existingType = await _webNovelTypeRepository.GetBySlugAsync(type.Name.ToSlug());
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

    [HttpPost("web-novel-types/search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelTypeDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<WebNovelTypeDTO>>> SearchWebNovelTypeByName([FromBody] WebNovelTypeSearchDTO type)
    {
        if (string.IsNullOrEmpty(type.Name))
        {
            return BadRequest($"Invalid Parameter: {nameof(type.Name)} Is Null Or Empty");
        }

        var webNovelTypes = await _webNovelTypeRepository.SearchByNameAsync(type.Name);
        if (webNovelTypes == null || !webNovelTypes.Any())
        {
            return NotFound("No WebNovel Types With The Given Criteria Found");
        }
        return Ok(webNovelTypes.ToDTO());
    }

    [HttpDelete("web-novel-types/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteWebNovelType(Guid id)
    {
        var webNovelType = await _webNovelTypeRepository.GetByIdAsync(id);
        if (webNovelType == null)
        {
            return NotFound($"No WebNovel Type With Id: {id} Found");
        }
        await _webNovelTypeRepository.DeleteAsync(webNovelType);
        await _unitOfWork.CommitAsync();
        return Accepted();
    }


    [HttpPut("web-novel-types/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebNovelTypeDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WebNovelTypeDTO>> UpdateWebNovelType(Guid id, [FromBody] WebNovelTypeUpdateDTO type)
    {
        if (string.IsNullOrEmpty(type.Name))
        {
            return BadRequest($"Invalid Parameter: {nameof(type.Name)} Is Null Or Empty");
        }

        var webNovelType = await _webNovelTypeRepository.GetByIdAsync(id);

        if (webNovelType == null)
        {
            return NotFound($"No WebNovel Type With Id: {id} Found");
        }

        try
        {
            webNovelType.SetName(type.Name);
            webNovelType.SetSlug(type.Name.ToSlug());

            _webNovelTypeRepository.MarkAsModified(webNovelType);
            await _unitOfWork.CommitAsync();

            return Ok(webNovelType.ToDTO());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("web-novel-types/{id:guid}/web-novels")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<WebNovelDTO>>> GetWebNovelTypeWebNovels(Guid id)
    {

        var webNovels = await _webNovelRepository.GetByTypeAsync(id);

        if (webNovels == null || !webNovels.Any())
        {
            return NoContent();
        }

        return Ok(webNovels.ToDTO());
    }

}