using MangaRead.Application.DTOs.System.File;

public class FileTypeOptions
{
    public FileType FileType { get; set; }
    public string RealPath { get; set; } = string.Empty;
    public string UrlPath { get; set; } = string.Empty;
    public long MaxFileSizeInBytes { get; set; }
    public string[] AllowedExtensions { get; set; } = Array.Empty<string>();

}