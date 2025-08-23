using MangaRead.Application.DTOs.System.File;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

public class FileService
{
    private readonly FileUploadOptions _fileUploadOptions;
    private readonly IBucketService _bucketService;
    private readonly HttpClient _httpClient = new HttpClient();

    public FileService(IOptions<FileUploadOptions> fileUploadOptions, IBucketService bucketService)
    {
        _bucketService = bucketService;
        _fileUploadOptions = fileUploadOptions.Value;
    }

    public string GetSanitizedFileName(string fileName)
    {

        fileName = UrlEncoder.Default.Encode(fileName);
        fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

        var creationTime = DateTime.Now.ToString("yyyyMMddHHmmss");

        var newName = $"{creationTime}_{fileName}";

        return newName;
    }

    private string GetRealBasePath(FileType fileType)
    {
        if (!_fileUploadOptions.FileTypeOptions.TryGetValue(fileType, out var fileTypeOptions))
        {
            throw new ArgumentException($"File type '{fileType}' is not configured.");
        }
        return fileTypeOptions.RealPath;

    }

    private string GetUrlBasePath(FileType fileType)
    {
        if (!_fileUploadOptions.FileTypeOptions.TryGetValue(fileType, out var fileTypeOptions))
        {
            throw new ArgumentException($"File type '{fileType}' is not configured.");
        }
        return fileTypeOptions.UrlPath;

    }


    public bool IsFileSizeValid(IFormFile file, FileType fileType)
    {
        return fileType switch
        {
            FileType.Image => file.Length <= _fileUploadOptions.FileTypeOptions[fileType].MaxFileSizeInBytes,
            FileType.Video => file.Length <= _fileUploadOptions.FileTypeOptions[fileType].MaxFileSizeInBytes,
            FileType.Text => file.Length <= _fileUploadOptions.FileTypeOptions[fileType].MaxFileSizeInBytes,
            _ => false
        };
    }

    public bool IsFileExtensionValid(string extension, FileType fileType)
    {
        return fileType switch
        {
            FileType.Image => Contains(_fileUploadOptions.FileTypeOptions[fileType].AllowedExtensions, extension),
            FileType.Video => Contains(_fileUploadOptions.FileTypeOptions[fileType].AllowedExtensions, extension),
            FileType.Text => Contains(_fileUploadOptions.FileTypeOptions[fileType].AllowedExtensions, extension),
            _ => false
        };
    }

    private bool Contains(string[] allowedExtensions, string v)
    {
        return allowedExtensions.Select(x => x.ToLower()).ToList().Contains(v.ToLower());
    }

    public async Task<FileDTO> DownloadFileAsync(Uri url, FileType fileType, string fileName, string? subFolder = null)
    {

        var sanitizedFileName = GetSanitizedFileName(fileName);

        var basePath = GetRealBasePath(fileType);

        if (subFolder != null)
        {
            basePath = $"{basePath}/{subFolder}";
        }

        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }

        var filePath = $"{basePath}/{sanitizedFileName}";

        try
        {
            using var resource = await _httpClient.GetStreamAsync(url);
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await resource.CopyToAsync(fileStream);
            }

            string fileKey = subFolder == null ? string.Empty : $"{subFolder}/{sanitizedFileName}";

            var bucketURL = await _bucketService.UploadFileToBucket(filePath, fileKey);

            File.Delete(filePath);

            return new FileDTO(bucketURL);
        }
        catch (HttpRequestException httpEx)
        {
            throw new HttpRequestException($"Error while downloading the file: {httpEx.Message}");
        }
        catch (IOException ioEx)
        {
            throw new IOException($"Error saving the file: {ioEx.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<FileDTO> SaveFile(IFormFile file, FileType fileType, string fileName, string? subFolder = null)
    {
        try
        {

            var sanitizedFileName = GetSanitizedFileName(fileName);
            var basePath = GetRealBasePath(fileType);

            if (subFolder != null)
            {
                basePath = Path.Combine(basePath, subFolder);
            }

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            var filePath = Path.Combine(basePath, sanitizedFileName);

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await file.CopyToAsync(stream);
                return new FileDTO(fileName);
            }
        }
        catch (IOException ioEx)
        {
            throw new IOException($"Error saving the file: {ioEx.Message}");
        }
    }
}
