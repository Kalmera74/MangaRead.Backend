using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities;

namespace MangaRead.Domain.Entities.Status;

public sealed class StatusEntity : Entity
{

    public string Name { get; private set; }
    public string Slug { get; private set; }

#pragma warning disable CS8618

    private StatusEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static StatusEntity Create(string name, string slug)
    {
        ValidateSlug(slug);
        ValidateName(name);

        var statusId = Guid.NewGuid();
        var status = new StatusEntity(statusId)
        {
            Name = name,
            Slug = slug
        };

        return status;
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

    private static void ValidateName(string statusName)
    {
        Guard.AgainstNullOrEmpty(statusName, nameof(statusName));
    }
    private static void ValidateSlug(string slug)
    {
        SlugValidator.ValidateSlug(slug);
    }
}