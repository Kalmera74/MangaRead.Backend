using MangaRead.Application.DTOs.System.Image;
using MangaRead.Application.Repositories.System.Image;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.System.Image;
using Microsoft.AspNetCore.Mvc;

namespace MangaRead.API.Controllers.System.Image;
[Route("/api/v1/")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageRepository _imageRepository;

    public ImageController(IUnitOfWork unitOfWork, IImageRepository imageRepository)
    {
        _unitOfWork = unitOfWork;
        _imageRepository = imageRepository;
    }

    [HttpGet("images")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ImageDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ImageDTO>>> GetImages()
    {
        var images = await _imageRepository.GetAllAsync();

        if (images == null || !images.Any())
        {
            return NotFound("No Images Found");
        }

        return Ok(images.ToDTO());
    }
    [HttpGet("images/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ImageDTO>> GetImageById(Guid id)
    {
        var image = await _imageRepository.GetByIdAsync(id);

        if (image == null)
        {
            return NotFound($"No Image With Id: {id} Found");
        }

        return Ok(image.ToDTO());
    }


    [HttpGet("images/page/{page:int}/size/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ImageDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ImageDTO>>> GetImagesPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }

        var images = await _imageRepository.GetAllPagedAsync(page, pageSize);

        if (images == null || !images.Any())
        {
            return NotFound("No Images Found");
        }
        return Ok(images.ToDTO());
    }


    [HttpDelete("images/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteImage(Guid id)
    {
        var image = await _imageRepository.GetByIdAsync(id);
        if (image == null)
        {
            return NotFound($"No Image With Id: {id} Found");
        }
        await _imageRepository.DeleteAsync(image);
        await _unitOfWork.CommitAsync();
        return Accepted();
    }

    [HttpPost("images")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImageDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImageDTO>> CreateImage([FromBody] ImageCreateDTO image)
    {
        if (string.IsNullOrEmpty(image.Url))
        {
            return BadRequest($"Invalid Parameter: {nameof(image.Url)} Is Null Or Empty");
        }

        var existingImage = await _imageRepository.GetByUrlAsync(image.Url);
        if (existingImage != null)
        {
            return Ok(existingImage.ToDTO());
        }

        try
        {
            ImageEntity newImage = ImageEntity.Create(image.Url);

            await _imageRepository.AddAsync(newImage);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetImageById), new { id = newImage.Id }, newImage.ToDTO());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("images/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImageDTO>> UpdateImage(Guid id, [FromBody] ImageUpdateDTO image)
    {
        ImageEntity? imageToUpdate = await _imageRepository.GetByIdAsync(id);

        if (imageToUpdate == null)
        {
            return NotFound($"No Image With Id: {id} Found");
        }

        try
        {
            imageToUpdate.SetUrl(image.Url);
            _imageRepository.MarkAsModified(imageToUpdate);

            await _unitOfWork.CommitAsync();

            return Ok(imageToUpdate.ToDTO());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("images/search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ImageDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ImageDTO>>> SearchImageByName([FromBody] ImageSearchDTO image)
    {
        if (string.IsNullOrEmpty(image.PartialUrl))
        {
            return BadRequest($"Invalid Search Criteria: {nameof(image.PartialUrl)}  Is Null Or Empty");
        }

        var images = await _imageRepository.SearchByUrlAsync(image.PartialUrl);

        if (images == null || !images.Any())
        {
            return NotFound($"No Image With The Given Criteria Found");
        }

        return Ok(images.ToDTO());
    }



}