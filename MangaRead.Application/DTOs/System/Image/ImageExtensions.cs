using MangaRead.Domain.Entities.System.Image;

namespace MangaRead.Application.DTOs.System.Image;
public static class ImageExtensions
{
    public static ImageDTO ToDTO(this ImageEntity imageEntity)
    {
        return new ImageDTO
        (
            imageEntity.Id,
            imageEntity.Url
        );
    }

    public static IEnumerable<ImageDTO> ToDTO(this IEnumerable<ImageEntity> imageEntities)
    {
        return imageEntities.Select(x => x.ToDTO());
    }

}