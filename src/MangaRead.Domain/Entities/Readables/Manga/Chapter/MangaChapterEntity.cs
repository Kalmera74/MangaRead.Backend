using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities;
using MangaRead.Domain.Entities.Readables.Manga.Chapter.Content;

namespace MangaRead.Domain.Entities.Readables.Manga.Chapter;

public sealed class MangaChapterEntity : Entity
{
    public string Title { get; private set; }
    public string Slug { get; private set; }

    public string? MetaTitle { get; private set; } = null;
    public string? MetaDescription { get; private set; } = null;
    public bool IsPublished { get; private set; }
    public MangaChapterEntity? PreviousChapter { get; private set; }
    public MangaChapterEntity? NextChapter { get; private set; }
    public List<MangaChapterContentEntity> Content { get; private set; } = new List<MangaChapterContentEntity>();

    public MangaEntity Manga { get; private set; }


#pragma warning disable CS8618
    private MangaChapterEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static MangaChapterEntity Create(MangaEntity manga, string title, string slug)
    {
        ValidateSlug(slug);
        ValidateManga(manga);
        ValidateTitle(title);

        var chapterId = Guid.NewGuid();
        var chapter = new MangaChapterEntity(chapterId)
        {
            Slug = $"{manga.Slug}-{slug}",
            Title = title
        };

        chapter.SetManga(manga);

        return chapter;
    }

    public void SetSlug(string slug)
    {
        ValidateSlug(slug);

        Slug = slug;
        SetUpdatedAt();
    }
    public void SetTitle(string title)
    {
        ValidateTitle(title);

        Title = title;
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


    public void SetPreviousChapter(MangaChapterEntity chapter)
    {
        ValidateChapter(chapter);

        if (chapter == PreviousChapter)
        {
            return;
        }
        PreviousChapter = chapter;
        PreviousChapter.SetNextChapter(this);
        SetUpdatedAt();
    }

    public void RemovePreviousChapter()
    {
        if (PreviousChapter == null)
        {
            return;
        }

        PreviousChapter.RemoveNextChapter();
        PreviousChapter = null;
        SetUpdatedAt();
    }

    public void SetNextChapter(MangaChapterEntity chapter)
    {
        ValidateChapter(chapter);
        if (chapter == NextChapter)
        {
            return;
        }

        NextChapter = chapter;
        NextChapter.SetPreviousChapter(this);
        SetUpdatedAt();
    }

    public void RemoveNextChapter()
    {
        if (NextChapter == null)
        {
            return;
        }

        NextChapter.RemovePreviousChapter();
        NextChapter = null;
        SetUpdatedAt();
    }


    public void AddContent(MangaChapterContentEntity content)
    {
        ValidateContent(content);

        if (Content.Contains(content))
        {
            return;
        }

        Content.Add(content);

        content.SetOrder(Content.Count);

        content.SetChapter(this);

        IsPublished = true;

        SetUpdatedAt();

    }

    public void ClearContent()
    {
        Content.Clear();
    }

    public void SetManga(MangaEntity manga)
    {
        ValidateManga(manga);

        if (Manga == manga)
        {
            return;
        }


        Manga?.RemoveChapter(this);

        Manga = manga;
        manga.AddChapter(this);

        SetUpdatedAt();

    }
    private static void ValidateManga(MangaEntity chapterManga)
    {
        Guard.AgainstNull(chapterManga, nameof(chapterManga));
    }
    private void ValidateChapter(MangaChapterEntity chapter)
    {
        Guard.AgainstNull(chapter, nameof(chapter));
        Guard.AgainstSelfReference(this, chapter, nameof(chapter));

    }
    private static void ValidateTitle(string chapterTitle)
    {
        Guard.AgainstNullOrEmpty(chapterTitle, nameof(chapterTitle));
    }

    private static void ValidateContent(MangaChapterContentEntity chapterContent)
    {
        Guard.AgainstNull(chapterContent, nameof(chapterContent));
    }
    private static void ValidateSlug(string slug)
    {
        SlugValidator.ValidateSlug(slug);
    }
    private static void ValidateMetaTitle(string metaTitle)
    {
        Guard.AgainstNullOrEmpty(metaTitle, nameof(metaTitle));
    }

    private static void ValidateMetaDescription(string contentMetaDescription)
    {
        Guard.AgainstNullOrEmpty(contentMetaDescription, nameof(contentMetaDescription));
    }


}
