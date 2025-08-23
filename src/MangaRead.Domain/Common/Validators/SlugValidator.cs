namespace MangaRead.Domain.Common.Validators;

public static class SlugValidator
{
    public static void ValidateSlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            throw new ArgumentException($"'{nameof(slug)}' cannot be null or empty.", nameof(slug));
        }
    }
}