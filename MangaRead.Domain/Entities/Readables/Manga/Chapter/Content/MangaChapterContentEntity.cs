using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities;
using MangaRead.Domain.Entities.System.Image;

namespace MangaRead.Domain.Entities.Readables.Manga.Chapter.Content;

public sealed class MangaChapterContentEntity : Entity
{

    public ImageEntity Item { get; private set; }
    public int Order { get; private set; }

    public MangaChapterEntity Chapter { get; private set; }

#pragma warning disable CS8618

    private MangaChapterContentEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618



    public static MangaChapterContentEntity Create(MangaChapterEntity chapter, ImageEntity items)
    {
        ValidateChapter(chapter);

        var contentId = Guid.NewGuid();
        var content = new MangaChapterContentEntity(contentId)
        {
            Item = items
        };


        content.SetChapter(chapter);


        return content;
    }



    public void SetChapter(MangaChapterEntity chapter)
    {
        ValidateChapter(chapter);

        if (Chapter == chapter)
        {
            return;
        }

        Chapter = chapter;
        chapter.AddContent(this);
        SetUpdatedAt();

    }
    public void SetItem(ImageEntity item)
    {
        ValidateContentItem(item);

        if (Item != item)
        {
            return;
        }

        Item = item;
        SetUpdatedAt();
    }

    public void SetOrder(int order)
    {
        ValidateOrder(order);

        if (Order == order)
        {
            return;
        }

        Order = order;
        SetUpdatedAt();
    }

    private void ValidateOrder(int order)
    {
        Guard.AgainstOutOfRange(order, nameof(order), 0, int.MaxValue);
    }

    private static void ValidateContentItem(ImageEntity contentItem)
    {
        Guard.AgainstNull(contentItem, nameof(contentItem));
    }

    private static void ValidateChapter(MangaChapterEntity contentChapter)
    {
        Guard.AgainstNull(contentChapter, nameof(contentChapter));
    }

}