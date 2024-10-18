using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using MangaRead.Domain.Common.Validators;

public static class SlugService
{


    public static string ToSlug(this string input)
    {
        SlugValidator.ValidateSlug(input);

        // Convert to lowercase
        string slug = input.ToLowerInvariant().Trim();

        // Remove diacritics (accents)
        slug = RemoveDiacritics(slug);

        // Replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s", "-");

        // Remove invalid characters
        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

        // Remove consecutive hyphens
        slug = Regex.Replace(slug, @"-{2,}", "-");

        return slug;
    }

    private static string RemoveDiacritics(string text)
    {
        string normalized = text.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();

        foreach (char c in normalized)
        {
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}