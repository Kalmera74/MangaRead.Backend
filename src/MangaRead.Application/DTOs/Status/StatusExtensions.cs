using MangaRead.Domain.Entities.Status;

namespace MangaRead.Application.DTOs.Status;
public static class MangaStatusExtensions
{
    public static StatusDTO ToDTO(this StatusEntity statusEntity)
    {

        return new StatusDTO
        (
            statusEntity.Id,
            statusEntity.Name

        );
    }

    public static IEnumerable<StatusDTO> ToDTO(this IEnumerable<StatusEntity> statusEntities)
    {
        return statusEntities.Select(x => x.ToDTO());
    }

}