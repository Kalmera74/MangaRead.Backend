using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities.System.User;

namespace MangaRead.Domain.Entities.Rating;

public sealed class RatingEntity : Entity
{

    public float StarCount { get; private set; }
    public UserEntity User { get; private set; }




#pragma warning disable CS8618

    private RatingEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static RatingEntity Create(UserEntity user, float StarCount)
    {
        ValidateUser(user);
        ValidateStarCount(StarCount);

        var ratingId = Guid.NewGuid();
        var rating = new RatingEntity(ratingId)
        {
            StarCount = StarCount,
            User = user
        };

        return rating;
    }

    public void SetStarCount(float starCount)
    {
        ValidateStarCount(starCount);
        StarCount = starCount;
        SetUpdatedAt();
    }

    private static void ValidateStarCount(float starCount)
    {
        if (starCount < 0 || starCount > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(starCount));
        }
    }


    private static void ValidateUser(UserEntity ratingUser)
    {
        Guard.AgainstNull(ratingUser, nameof(ratingUser));
    }


}