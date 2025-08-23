namespace MangaRead.Domain.Common;

public interface ISlugService
{
    public string Slugify(string value);
}