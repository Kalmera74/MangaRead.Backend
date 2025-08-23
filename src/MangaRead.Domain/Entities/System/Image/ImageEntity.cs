using MangaRead.Domain.Common.Validators;

namespace MangaRead.Domain.Entities.System.Image;

public sealed class ImageEntity : Entity
{
    public string Url { get; private set; }

#pragma warning disable CS8618

    private ImageEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static ImageEntity Create(string url)
    {
        ValidateUrl(url);

        var imageGuid = Guid.NewGuid();
        var image = new ImageEntity(imageGuid)
        {
            Url = url
        };

        return image;
    }

    public void SetUrl(string url)
    {
        ValidateUrl(url);

        Url = url;
        SetUpdatedAt();
    }

    private static void ValidateUrl(string url)
    {
        Guard.AgainstNullOrEmpty(url, nameof(url));

        // if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        // {
        //     throw new ArgumentException($"{nameof(url)} is not a valid URL");
        // }
    }
}