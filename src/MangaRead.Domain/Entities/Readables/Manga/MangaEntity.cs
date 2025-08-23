using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities;
using MangaRead.Domain.Entities.Author;
using MangaRead.Domain.Entities.Genre;
using MangaRead.Domain.Entities.Rating;
using MangaRead.Domain.Entities.Readables.Manga.Chapter;
using MangaRead.Domain.Entities.Readables.Manga.Type;
using MangaRead.Domain.Entities.Readables.WebNovel;
using MangaRead.Domain.Entities.Status;
using MangaRead.Domain.Entities.System.Image;

namespace MangaRead.Domain.Entities.Readables.Manga;

public sealed class MangaEntity : Entity
{

    public string Title { get; private set; }
    public string Slug { get; private set; }
    public string Description { get; private set; }
    public int ViewCount { get; private set; } = 0;
    public int SeasonCount { get; private set; } = 0;
    public float Rating { get; private set; } = 0;
    public bool IsPublished { get; private set; } = false;

    public string? MetaTitle { get; private set; } = null;
    public string? MetaDescription { get; private set; } = null;


    public ImageEntity CoverImage { get; private set; }
    public StatusEntity Status { get; private set; }
    public MangaTypeEntity Type { get; private set; }
    public WebNovelEntity? WebNovel { get; private set; }


    public List<AuthorEntity> Authors { private set; get; } = new List<AuthorEntity>();
    public List<MangaChapterEntity> Chapters { private set; get; } = new List<MangaChapterEntity>();
    public List<RatingEntity> Ratings { private set; get; } = new List<RatingEntity>();
    public List<GenreEntity> Genres { private set; get; } = new List<GenreEntity>();

    private const double SEASON_DIVIDER = 100d;

#pragma warning disable CS8618

    private MangaEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static MangaEntity Create(string title,
                                    string slug,
                                    string description,
                                    AuthorEntity[] authors,
                                    ImageEntity coverImage,
                                    StatusEntity status,
                                    MangaTypeEntity type,
                                    IList<GenreEntity> genres)
    {

        ValidateTitle(title);
        ValidateSlug(slug);
        ValidateDescription(description);
        ValidateAuthors(authors);
        ValidateCoverImage(coverImage);
        ValidateStatus(status);
        ValidateType(type);
        ValidateGenres(genres);

        var mangaID = Guid.NewGuid();
        var manga = new MangaEntity(mangaID)
        {
            Title = title,
            Slug = slug,
            Description = description,
            CoverImage = coverImage,
            Status = status,
            Type = type,
        };

        foreach (var author in authors)
        {
            manga.AddAuthor(author);
        }

        foreach (var genre in genres)
        {
            manga.AddGenre(genre);
        }

        manga.Rating = 2.0f + Random.Shared.NextSingle() * (5.0f - 2.0f);
        manga.ViewCount = Random.Shared.Next(10000, 100000);


        return manga;

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
    public void SetSlug(string slug)
    {
        ValidateSlug(slug);

        Slug = slug;
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

    public void SetStatus(StatusEntity status)
    {
        ValidateStatus(status);
        Status = status;
        SetUpdatedAt();
    }

    public void SetType(MangaTypeEntity type)
    {
        ValidateType(type);
        Type = type;
        SetUpdatedAt();
    }

    public void IncreaseViewCountByOne()
    {
        ViewCount++;
    }
    private void CalculateSeasonCount()
    {
        SeasonCount = Math.Max(1, (int)Math.Ceiling(Chapters.Count / SEASON_DIVIDER));
    }
    public void AddChapter(MangaChapterEntity chapter)
    {
        ValidateChapter(chapter);

        if (Chapters.Contains(chapter))
        {
            return;
        }

        Chapters.Add(chapter);
        chapter.SetManga(this);
        CalculateSeasonCount();

        IsPublished = Chapters.Count > 0;

        SetUpdatedAt();
    }



    public void RemoveChapter(MangaChapterEntity chapter)
    {
        ValidateChapter(chapter);

        if (!Chapters.Contains(chapter))
        {
            return;
        }

        Chapters.Remove(chapter);
        CalculateSeasonCount();

        IsPublished = Chapters.Count > 0;

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

    public void SetWebNovel(WebNovelEntity webNovel)
    {
        ValidateWebNovel(webNovel);

        if (WebNovel == webNovel)
        {
            return;
        }

        if (WebNovel != null)
        {
            WebNovel.RemoveManga();
        }

        WebNovel = webNovel;
        WebNovel.SetManga(this);
        SetUpdatedAt();

    }
    public void RemoveWebNovel()
    {
        if (WebNovel == null)
        {
            return;
        }

        WebNovel?.RemoveManga();
        WebNovel = null;
    }

    private void CalculateRating()
    {
        float finalRating = 0.0f;
        foreach (var rating in Ratings)
        {
            finalRating += rating.StarCount;
        }

        Rating = finalRating / Ratings.Count;
        SetUpdatedAt();
    }
    private static void ValidateGenres(IList<GenreEntity> mangaGenres)
    {
        Guard.AgainstNull(mangaGenres, nameof(mangaGenres));
    }

    private static void ValidateType(MangaTypeEntity mangaType)
    {
        Guard.AgainstNull(mangaType, nameof(mangaType));
    }

    private static void ValidateStatus(StatusEntity mangaStatus)
    {
        Guard.AgainstNull(mangaStatus, nameof(mangaStatus));
    }

    private static void ValidateCoverImage(ImageEntity mangaCoverImage)
    {
        Guard.AgainstNull(mangaCoverImage, nameof(mangaCoverImage));
    }

    private static void ValidateDescription(string mangaDescription)
    {
        Guard.AgainstNullOrEmpty(mangaDescription, nameof(mangaDescription));
    }

    private static void ValidateSlug(string slug)
    {
        SlugValidator.ValidateSlug(slug);
    }

    private static void ValidateTitle(string mangaTitle)
    {
        Guard.AgainstNullOrEmpty(mangaTitle, nameof(mangaTitle));
    }

    private static void ValidateAuthors(IList<AuthorEntity> mangaAuthors)
    {
        foreach (var author in mangaAuthors)
        {
            ValidateAuthor(author);
        }
    }
    private static void ValidateAuthor(AuthorEntity mangaAuthor)
    {
        Guard.AgainstNull(mangaAuthor, nameof(mangaAuthor));
    }
    private static void ValidateGenre(GenreEntity mangaGenre)
    {
        Guard.AgainstNull(mangaGenre, nameof(mangaGenre));
    }
    private static void ValidateWebNovel(WebNovelEntity mangaWebNovel)
    {
        Guard.AgainstNull(mangaWebNovel, nameof(mangaWebNovel));
    }
    private static void ValidateChapter(MangaChapterEntity mangaChapter)
    {
        Guard.AgainstNull(mangaChapter, nameof(mangaChapter));
    }

    private static void ValidateMetaDescription(string webNovelMetaDescription)
    {
        Guard.AgainstNullOrEmpty(webNovelMetaDescription, nameof(webNovelMetaDescription));
    }

    private static void ValidateMetaTitle(string webNovelMetaTitle)
    {
        Guard.AgainstNullOrEmpty(webNovelMetaTitle, nameof(webNovelMetaTitle));
    }

}

