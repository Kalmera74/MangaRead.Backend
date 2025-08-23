using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities;
namespace MangaRead.Domain.Entities.Author;

public sealed class AuthorEntity : Entity
{

    public string Name { get; private set; }
    public string Slug { get; private set; }

#pragma warning disable CS8618

    private AuthorEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static AuthorEntity Create(string name, string slug)
    {
        ValidateSlug(slug);
        ValidateName(name);

        var authorId = Guid.NewGuid();
        var author = new AuthorEntity(authorId)
        {
            Name = name,
            Slug = slug
        };

        return author;
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
    private static void ValidateName(string authorName)
    {
        Guard.AgainstNullOrEmpty(authorName, nameof(authorName));
    }

    private static void ValidateSlug(string authorSlug)
    {
        SlugValidator.ValidateSlug(authorSlug);
    }
}
