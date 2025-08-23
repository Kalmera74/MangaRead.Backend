using MangaRead.Application.DTOs.System.File;
using Microsoft.AspNetCore.Mvc;
using Serilog;
namespace MangaRead.API.Controllers.System.File;

[Route("/api/v1/")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;
    public FileController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("files/uploaded")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FileDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileDTO?>> CreateUploadedFile([FromForm] FileCreateDTO fileData)
    {

        if (fileData.File == null)
        {
            return BadRequest("File Is Null");
        }
        var fileExtension = fileData.Extension ?? Path.GetExtension(fileData.File.FileName);

        if (!_fileService.IsFileExtensionValid(fileExtension, fileData.FileType))
        {
            return BadRequest("File Extension Is Not Allowed");
        }

        if (!_fileService.IsFileSizeValid(fileData.File, fileData.FileType))
        {
            return BadRequest("File Size Is Not Allowed");
        }

        try
        {

            var fileName = string.IsNullOrEmpty(fileData.Name)
                ? Path.GetFileNameWithoutExtension(fileData.File.FileName) + fileExtension
                : fileData.Name + fileExtension;


            FileDTO createdFile = await _fileService.SaveFile(fileData.File, fileData.FileType, fileName, fileData.SubFolder);


            return Ok(createdFile);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (IOException ioEx)
        {
            return StatusCode(500, ioEx.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    [HttpPost("files/downloaded")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FileDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileDTO?>> CreateDownloadedFileAsync([FromBody] FileDownloadDTO fileData)
    {
        if (string.IsNullOrEmpty(fileData.Url))
        {
            return BadRequest("Url is null or empty");
        }

        var fileExtension = (fileData.Extension ?? Path.GetExtension(fileData.Url)).Replace(".", "");


        if (!_fileService.IsFileExtensionValid(fileExtension, fileData.FileType))
        {
            return BadRequest("File Extension Is Not Allowed");
        }

        if (!Uri.TryCreate(fileData.Url, UriKind.Absolute, out var resourceUri) ||
            resourceUri.Scheme != Uri.UriSchemeHttp && resourceUri.Scheme != Uri.UriSchemeHttps)
        {
            return BadRequest("Invalid Url format");
        }

        try
        {

            var fileName = string.IsNullOrEmpty(fileData.Name)
                ? $"{Path.GetFileNameWithoutExtension(fileData.Url)}.{fileExtension}"
                : $"{fileData.Name}.{fileExtension}";

            var result = await _fileService.DownloadFileAsync(resourceUri, fileData.FileType, fileName, fileData.SubFolder);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error($"Unexpected error while downloading file. Reason: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }


}