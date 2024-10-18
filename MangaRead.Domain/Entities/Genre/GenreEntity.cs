using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities;

namespace MangaRead.Domain.Entities.Genre;

public sealed class GenreEntity : Entity
{

    public string Name { get; private set; }
    public string Slug { get; private set; }

#pragma warning disable CS8618

    private GenreEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static GenreEntity Create(string name, string slug)
    {
        ValidateSlug(slug);
        ValidateName(name);

        var badgeGuid = Guid.NewGuid();
        var genre = new GenreEntity(badgeGuid)
        {
            Name = name,
            Slug = slug
        };

        return genre;

    }




    public void SetName(string name)
    {
        ValidateName(name);

        Name = name;
        SetUpdatedAt();
    }

    public void SetSlug(string slug)
    {
        ValidateSlug(slug);

        Slug = slug;
        SetUpdatedAt();
    }
    private static void ValidateName(string genreName)
    {
        Guard.AgainstNullOrEmpty(genreName, nameof(genreName));
    }

    private static void ValidateSlug(string genreSlug)
    {
        SlugValidator.ValidateSlug(genreSlug);
    }


}