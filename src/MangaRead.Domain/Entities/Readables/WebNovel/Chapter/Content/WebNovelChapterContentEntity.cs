using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities;
using MangaRead.Domain.Entities.Readables.WebNovel.Chapter;

namespace MangaRead.Domain.Entities.Readables.WebNovel.Chapter.Content;

public sealed class WebNovelChapterContentEntity : Entity
{

    public string Title { get; private set; }
    public string Body { get; private set; }
    public string? MetaTitle { get; private set; } = null;
    public string? MetaDescription { get; private set; } = null;
    public WebNovelChapterEntity Chapter { get; private set; }


#pragma warning disable CS8618

    private WebNovelChapterContentEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static WebNovelChapterContentEntity Create(string title, string contentBody, WebNovelChapterEntity chapter)
    {
        ValidateTitle(title);
        ValidateContentBody(contentBody);
        ValidateChapter(chapter);

        var contentId = Guid.NewGuid();
        var content = new WebNovelChapterContentEntity(contentId)
        {
            Title = title,
            Body = contentBody,
        };
        content.SetChapter(chapter);

        return content;
    }


    public void SetTitle(string title)
    {
        ValidateTitle(title);

        Title = title;
        SetUpdatedAt();

    }

    public void SetContentBody(string contentBody)
    {
        ValidateContentBody(contentBody);

        Body = contentBody;
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

    public void SetChapter(WebNovelChapterEntity chapter)
    {
        ValidateChapter(chapter);

        if (Chapter != chapter)
        {
            Chapter = chapter;
            chapter.SetContent(this);
            SetUpdatedAt();

        }
    }
    private static void ValidateMetaDescription(string webNovelChapterContentMetaDescription)
    {
        Guard.AgainstNullOrEmpty(webNovelChapterContentMetaDescription, nameof(webNovelChapterContentMetaDescription));
    }

    private static void ValidateMetaTitle(string webNovelChapterContentMetaTitle)
    {
        Guard.AgainstNullOrEmpty(webNovelChapterContentMetaTitle, nameof(webNovelChapterContentMetaTitle));
    }

    private static void ValidateChapter(WebNovelChapterEntity webNovelChapterContentChapter)
    {
        Guard.AgainstNull(webNovelChapterContentChapter, nameof(webNovelChapterContentChapter));
    }

    private static void ValidateContentBody(string webNovelChapterContentContentBody)
    {
        Guard.AgainstNullOrEmpty(webNovelChapterContentContentBody, nameof(webNovelChapterContentContentBody));
    }

    private static void ValidateTitle(string webNovelChapterContentTitle)
    {
        Guard.AgainstNullOrEmpty(webNovelChapterContentTitle, nameof(webNovelChapterContentTitle));
    }
}
