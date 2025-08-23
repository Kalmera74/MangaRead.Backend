using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities;
using MangaRead.Domain.Entities.Author;
using MangaRead.Domain.Entities.Genre;
using MangaRead.Domain.Entities.Rating;
using MangaRead.Domain.Entities.Readables.Manga;
using MangaRead.Domain.Entities.Readables.WebNovel.Chapter;
using MangaRead.Domain.Entities.Readables.WebNovel.Type;
using MangaRead.Domain.Entities.Status;
using MangaRead.Domain.Entities.System.Image;

namespace MangaRead.Domain.Entities.Readables.WebNovel;

public sealed class WebNovelEntity : Entity
{

    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Slug { get; private set; }
    public int ViewCount { get; private set; } = 0;
    public int SeasonCount { get; private set; } = 0;
    public float Rating { get; private set; } = 0;
    public string? MetaTitle { get; private set; } = null;
    public string? MetaDescription { get; private set; } = null;


    public ImageEntity CoverImage { get; private set; }
    public MangaEntity? Manga { get; private set; }
    public StatusEntity Status { get; private set; }
    public WebNovelTypeEntity Type { get; private set; }

    public List<AuthorEntity> Authors { get; private set; } = new List<AuthorEntity>();
    public List<WebNovelChapterEntity> Chapters { get; private set; } = new List<WebNovelChapterEntity>();
    public List<RatingEntity> Ratings { get; private set; } = new List<RatingEntity>();
    public List<GenreEntity> Genres { get; private set; } = new List<GenreEntity>();

    private const int SEASON_DIVIDER = 100;



#pragma warning disable CS8618

    private WebNovelEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static WebNovelEntity Create(string title, string description, ImageEntity coverImage, AuthorEntity[] authors, IList<GenreEntity> genres, StatusEntity status, string slug)
    {
        ValidateTitle(title);
        ValidateDescription(description);
        ValidateCoverImage(coverImage);
        ValidateAuthors(authors);
        ValidateGenres(genres);
        ValidateStatus(status);
        ValidateSlug(slug);


        var webNovelId = Guid.NewGuid();
        var webNovel = new WebNovelEntity(webNovelId)
        {
            Title = title,
            Description = description,
            CoverImage = coverImage,
            Status = status,
            Slug = slug
        };

        foreach (var author in authors)
        {
            webNovel.AddAuthor(author);
        }

        foreach (var genre in genres)
        {
            webNovel.AddGenre(genre);
        }

        return webNovel;
    }



    public void SetTitle(string title)
    {
        ValidateTitle(title);

        Title = title;
        SetUpdatedAt();
    }
    public void SetDescription(string description)
    {
        ValidateDescription(description);

        Description = description;
        SetUpdatedAt();

    }

    public void SetMetaTitle(string metaTitle)
    {
        ValidateMetaTitle(metaTitle);

        MetaTitle = metaTitle;
        SetUpdatedAt();

    }

    public void SetMetaDescription(string metaDescription)
    {
        ValidateMetaDescription(metaDescription);

        MetaDescription = metaDescription;
        SetUpdatedAt();

    }
    public void SetCoverImage(ImageEntity coverImage)
    {
        ValidateCoverImage(coverImage);

        CoverImage = coverImage;
        SetUpdatedAt();
    }
    public void AddAuthor(AuthorEntity author)
    {
        ValidateAuthor(author);
        if (Authors.Contains(author))
        {
            return;
        }
        Authors.Add(author);
        SetUpdatedAt();
    }

    public void RemoveAuthor(AuthorEntity author)
    {
        if (!Authors.Contains(author))
        {
            return;
        }
        Authors.Remove(author);
        SetUpdatedAt();
    }

    public void AddGenre(GenreEntity genre)
    {
        ValidateGenre(genre);

        if (Genres.Contains(genre))
        {
            return;
        }

        Genres.Add(genre);
        SetUpdatedAt();
    }
    public void RemoveGenre(GenreEntity genre)
    {
        ValidateGenre(genre);

        if (!Genres.Contains(genre))
        {
            return;
        }

        Genres.Remove(genre);
        SetUpdatedAt();
    }

    public void SetStatus(StatusEntity status)
    {
        ValidateStatus(status);
        Status = status;

        SetUpdatedAt();

    }
    public void SetType(WebNovelTypeEntity type)
    {
        ValidateType(type);
        Type = type;
        SetUpdatedAt();
    }
    public void SetSlug(string slug)
    {
        ValidateSlug(slug);

        Slug = slug;
        SetUpdatedAt();
    }
    public void AddChapter(WebNovelChapterEntity chapter)
    {
        ValidateChapter(chapter);

        if (Chapters.Contains(chapter))
        {
            return;
        }

        Chapters.Add(chapter);
        chapter.SetWebNovel(this);

        SetUpdatedAt();
        CalculateSeasonCount();
    }
    public void RemoveChapter(WebNovelChapterEntity chapter)
    {
        ValidateChapter(chapter);

        if (!Chapters.Contains(chapter))
        {
            return;
        }

        Chapters.Remove(chapter);

        CalculateSeasonCount();
        SetUpdatedAt();
    }
    public void IncrementViewCountByOne()
    {
        ViewCount++;
    }
    public void CalculateSeasonCount()
    {
        SeasonCount = Chapters.Count / SEASON_DIVIDER;
    }

    public void SetManga(MangaEntity manga)
    {
        ValidateManga(manga);

        if (Manga == manga)
        {
            return;
        }
        if (Manga != null)
        {
            Manga.RemoveWebNovel();
        }

        Manga = manga;
        Manga.SetWebNovel(this);
        SetUpdatedAt();
    }
    public void AddRating(RatingEntity rating)
    {
        if (Ratings.Contains(rating))
        {
            return;
        }

        Ratings.Add(rating);
        CalculateRating();
    }
    public void RemoveRating(RatingEntity rating)
    {
        if (!Ratings.Contains(rating))
        {
            return;
        }

        Ratings.Remove(rating);
        CalculateRating();
    }
    private void CalculateRating()
    {
        float calculatedRating = 0.0f;
        foreach (var rating in Ratings)
        {
            calculatedRating += rating.StarCount;
        }
        Rating = calculatedRating / Ratings.Count;
        SetUpdatedAt();
    }

    public void RemoveManga()
    {
        if (Manga == null)
        {
            return;
        }

        Manga.RemoveWebNovel();
        Manga = null;
    }
    private static void ValidateManga(MangaEntity webNovelManga)
    {
        Guard.AgainstNull(webNovelManga, nameof(webNovelManga));
    }

    private static void ValidateChapter(WebNovelChapterEntity webNobelChapter)
    {
        Guard.AgainstNull(webNobelChapter, nameof(webNobelChapter));

    }

    private static void ValidateStatus(StatusEntity status)
    {
        Guard.AgainstNull(status, nameof(status));
    }

    private static void ValidateGenres(IList<GenreEntity> genres)
    {
        Guard.AgainstNull(genres, nameof(genres));
    }
    private static void ValidateGenre(GenreEntity genre)
    {
        Guard.AgainstNull(genre, nameof(genre));
    }
    private static void ValidateAuthors(IList<AuthorEntity> mangaAuthors)
    {
        foreach (var author in mangaAuthors)
        {
            ValidateAuthor(author);
        }
    }
    private static void ValidateAuthor(AuthorEntity webNovelAuthor)
    {
        Guard.AgainstNull(webNovelAuthor, nameof(webNovelAuthor));
    }

    private static void ValidateCoverImage(ImageEntity webNovelCoverImage)
    {
        Guard.AgainstNull(webNovelCoverImage, nameof(webNovelCoverImage));
    }

    private static void ValidateTitle(string webNovelTitle)
    {
        Guard.AgainstNullOrEmpty(webNovelTitle, nameof(webNovelTitle));
    }
    private static void ValidateDescription(string webNovelDescription)
    {
        Guard.AgainstNullOrEmpty(webNovelDescription, nameof(webNovelDescription));
    }
    private static void ValidateSlug(string slug)
    {
        SlugValidator.ValidateSlug(slug);
    }

    private static void ValidateMetaDescription(string webNovelMetaDescription)
    {
        Guard.AgainstNullOrEmpty(webNovelMetaDescription, nameof(webNovelMetaDescription));
    }

    private static void ValidateMetaTitle(string webNovelMetaTitle)
    {
        Guard.AgainstNullOrEmpty(webNovelMetaTitle, nameof(webNovelMetaTitle));
    }
    private static void ValidateType(WebNovelTypeEntity webNovelType)
    {
        Guard.AgainstNull(webNovelType, nameof(webNovelType));
    }
}