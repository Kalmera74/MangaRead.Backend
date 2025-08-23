using MangaRead.Domain.Entities.Author;
using MangaRead.Domain.Entities.Genre;
using MangaRead.Domain.Entities.Rating;
using MangaRead.Domain.Entities.Readables.Manga;
using MangaRead.Domain.Entities.Readables.Manga.Chapter;
using MangaRead.Domain.Entities.Readables.Manga.Chapter.Content;
using MangaRead.Domain.Entities.Readables.Manga.Type;
using MangaRead.Domain.Entities.Readables.WebNovel;
using MangaRead.Domain.Entities.Readables.WebNovel.Chapter;
using MangaRead.Domain.Entities.Readables.WebNovel.Chapter.Content;
using MangaRead.Domain.Entities.Readables.WebNovel.Type;
using MangaRead.Domain.Entities.Status;
using MangaRead.Domain.Entities.System.Image;
using MangaRead.Domain.Entities.System.User;
using Microsoft.EntityFrameworkCore;
namespace MangaRead.Infrastructure.DbContexts;
public class MangaDbContext : DbContext
{

    public MangaDbContext() : base() { }
    public MangaDbContext(DbContextOptions<MangaDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(MangaDbContext).Assembly);

        base.OnModelCreating(builder);
    }


    public DbSet<AuthorEntity> Authors { get; set; }
    public DbSet<GenreEntity> Genres { get; set; }
    public DbSet<ImageEntity> Images { get; set; }
    public DbSet<MangaChapterContentEntity> MangaChapterContents { get; set; }
    public DbSet<MangaChapterEntity> MangaChapters { get; set; }
    public DbSet<RatingEntity> Ratings { get; set; }
    public DbSet<StatusEntity> Statuses { get; set; }
    public DbSet<MangaTypeEntity> MangaTypes { get; set; }
    public DbSet<WebNovelChapterContentEntity> WebNovelChapterContents { get; set; }
    public DbSet<WebNovelChapterEntity> WebNovelChapters { get; set; }
    public DbSet<WebNovelEntity> WebNovels { get; set; }
    public DbSet<WebNovelTypeEntity> WebNovelTypes { get; set; }
    public DbSet<MangaEntity> Mangas { get; set; }
    public DbSet<RoleEntity> UserRoles { get; set; }
    public DbSet<UserEntity> Users { get; set; }





}
