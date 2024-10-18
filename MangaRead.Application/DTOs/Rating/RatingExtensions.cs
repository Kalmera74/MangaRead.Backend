using MangaRead.Domain.Entities.Rating;

namespace MangaRead.Application.DTOs.Rating;
public static class RatingExtensions
{
    public static RatingDTO ToDTO(this RatingEntity mangaRatingEntity)
    {
        return new RatingDTO
        (
            mangaRatingEntity.Id,
            mangaRatingEntity.User.Id,
            mangaRatingEntity.StarCount
        );
    }

    public static IEnumerable<RatingDTO> ToDTO(this IEnumerable<RatingEntity> mangaRatingEntities)
    {
        return mangaRatingEntities.Select(x => x.ToDTO());
    }
}