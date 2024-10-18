using MangaRead.Application.DTOs.System.File;

public class FileUploadOptions
{
    public List<FileTypeOptions> FileOptions { get; set; } = new List<FileTypeOptions>();
    public Dictionary<FileType, FileTypeOptions> FileTypeOptions => FileOptions.ToDictionary(opt => opt.FileType, opt => opt);



}