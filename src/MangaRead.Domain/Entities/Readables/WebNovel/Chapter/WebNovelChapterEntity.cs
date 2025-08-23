using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities;
using MangaRead.Domain.Entities.Readables.WebNovel.Chapter.Content;

namespace MangaRead.Domain.Entities.Readables.WebNovel.Chapter;
public sealed class WebNovelChapterEntity : Entity
{

    public string Title { get; private set; }
    public string Slug { get; private set; }
    public WebNovelChapterEntity? PreviousChapter { get; private set; }
    public WebNovelChapterEntity? NextChapter { get; private set; }
    public WebNovelChapterContentEntity? Content { get; private set; }
    public WebNovelEntity WebNovel { get; private set; }


#pragma warning disable CS8618

    private WebNovelChapterEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static WebNovelChapterEntity Create(WebNovelEntity webNovel, string title, string slug)
    {
        ValidateSlug(slug);
        ValidateWebNovel(webNovel);
        ValidateTitle(title);


        var chapterId = Guid.NewGuid();
        var chapter = new WebNovelChapterEntity(chapterId)
        {
            Title = title,
            Slug = $"{webNovel.Slug}-{slug}"
        };

        chapter.SetWebNovel(webNovel);

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

    public void SetPreviousChapter(WebNovelChapterEntity chapter)
    {
        ValidateChapter(chapter);

        if (PreviousChapter == chapter)
        {
            return;
        }

        PreviousChapter = chapter;
        SetUpdatedAt();
    }

    public void SetNextChapter(WebNovelChapterEntity chapter)
    {
        ValidateChapter(chapter);

        if (NextChapter == chapter)
        {
            return;
        }

        NextChapter = chapter;
        SetUpdatedAt();
    }

    public void SetContent(WebNovelChapterContentEntity content)
    {
        ValidateContent(content);

        if (Content == content)
        {
            return;
        }

        Content = content;
        content.SetChapter(this);
        SetUpdatedAt();
    }


    public void SetWebNovel(WebNovelEntity webNovel)
    {
        ValidateWebNovel(webNovel);

        if (WebNovel == webNovel)
        {
            return;

        }

        WebNovel.RemoveChapter(this);

        WebNovel = webNovel;
        WebNovel.AddChapter(this);

        SetUpdatedAt();
    }
    private static void ValidateWebNovel(WebNovelEntity webNovelChapterWebNovel)
    {
        Guard.AgainstNull(webNovelChapterWebNovel, nameof(webNovelChapterWebNovel));
    }
    private void ValidateChapter(WebNovelChapterEntity webNovelChapterChapter)
    {
        Guard.AgainstNull(webNovelChapterChapter, nameof(webNovelChapterChapter));
        Guard.AgainstSelfReference(this, webNovelChapterChapter, nameof(webNovelChapterChapter));
    }
    private static void ValidateTitle(string webNovelChapterTitle)
    {
        Guard.AgainstNullOrEmpty(webNovelChapterTitle, nameof(webNovelChapterTitle));
    }

    private static void ValidateContent(WebNovelChapterContentEntity webNovelChapterContent)
    {
        Guard.AgainstNull(webNovelChapterContent, nameof(webNovelChapterContent));
    }
    private static void ValidateSlug(string slug)
    {
        Guard.AgainstNullOrEmpty(slug, nameof(slug));
    }
}
