using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities;

namespace MangaRead.Domain.Entities.Readables.WebNovel.Type;

public sealed class WebNovelTypeEntity : Entity
{
    public string Name { get; private set; }
    public string Slug { get; private set; }



#pragma warning disable CS8618

    private WebNovelTypeEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static WebNovelTypeEntity Create(string name, string slug)
    {
        ValidateSlug(slug);
        ValidateName(name);

        var typeId = Guid.NewGuid();
        var type = new WebNovelTypeEntity(typeId)
        {
            Name = name,
            Slug = slug
        };

        return type;
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




    private static void ValidateName(string typeName)
    {
        Guard.AgainstNullOrEmpty(typeName, nameof(typeName));
    }
    private static void ValidateSlug(string slug)
    {
        SlugValidator.ValidateSlug(slug);
    }
}