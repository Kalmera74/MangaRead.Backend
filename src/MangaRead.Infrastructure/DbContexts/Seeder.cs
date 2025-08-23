using MangaRead.Domain.Entities.Author;
using MangaRead.Domain.Entities.Genre;
using MangaRead.Domain.Entities.Readables.Manga;
using MangaRead.Domain.Entities.Readables.Manga.Chapter;
using MangaRead.Domain.Entities.Readables.Manga.Chapter.Content;
using MangaRead.Domain.Entities.Readables.Manga.Type;
using MangaRead.Domain.Entities.Readables.WebNovel.Type;
using MangaRead.Domain.Entities.Status;
using MangaRead.Domain.Entities.System.Image;
using MangaRead.Infrastructure.DbContexts;
using Microsoft.Extensions.DependencyInjection;

public static class Seeder
{
    public static void Seed(this IServiceProvider serviceProvider)
    {

        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<MangaDbContext>();

            context.Database.EnsureCreated();


            SeedGenres(context);
            SeedStatuses(context);
            SeedAuthors(context);
            SeedMangaTypes(context);
            SeedWebNovelTypes(context);
            SeedImages(context);
            SeedMangas(context);
            SeedMangaChapters(context);
            SeedMangaChapterContents(context);

        }


    }

    private static void SeedGenres(MangaDbContext context)
    {
        if (context.Genres.Any()) { return; }

        var genres = new List<GenreEntity>
        {
            GenreEntity.Create( "Action","Action".ToSlug() ),
            GenreEntity.Create( "Adventure","Adventure".ToSlug() ),
            GenreEntity.Create( "Romance","Romance".ToSlug() ),
            GenreEntity.Create( "Comedy","Comedy".ToSlug() ),
            GenreEntity.Create( "Drama","Drama".ToSlug() ),
            GenreEntity.Create( "Fantasy","Fantasy".ToSlug() ),
            GenreEntity.Create( "Science Fiction","Science Fiction".ToSlug() ),
            GenreEntity.Create( "Mystery","Mystery".ToSlug() ),
            GenreEntity.Create( "Thriller","Thriller".ToSlug() ),
            GenreEntity.Create( "Horror","Horror".ToSlug() ),
            GenreEntity.Create( "Historical","Historical".ToSlug() ),
            GenreEntity.Create( "Sports","Sports".ToSlug() ),
            GenreEntity.Create( "Slice of Life","Slice of Life".ToSlug() ),
            GenreEntity.Create( "Supernatural","Supernatural".ToSlug() ),
            GenreEntity.Create( "Psychological","Psychological".ToSlug() ),
            GenreEntity.Create( "School","School".ToSlug() ),
            GenreEntity.Create( "Mecha","Mecha".ToSlug() ),
            GenreEntity.Create( "Music","Music".ToSlug() ),
            GenreEntity.Create( "Seinen","Seinen".ToSlug() ),
            GenreEntity.Create( "Shoujo","Shoujo".ToSlug() ),
            GenreEntity.Create( "Shounen","Shounen".ToSlug() ),
            GenreEntity.Create( "Josei","Josei".ToSlug() ),
            GenreEntity.Create( "Harem","Harem".ToSlug() ),
            GenreEntity.Create( "Ecchi","Ecchi".ToSlug() ),
            GenreEntity.Create( "Isekai","Isekai".ToSlug() ),
            GenreEntity.Create( "Romantic Comedy","Romantic Comedy".ToSlug() ),
            GenreEntity.Create( "Gore","Gore".ToSlug() ),
            GenreEntity.Create( "Vampire","Vampire".ToSlug() ),
            GenreEntity.Create( "Samurai","Samurai".ToSlug() ),
            GenreEntity.Create( "Demons","Demons".ToSlug() ),
            GenreEntity.Create( "Martial Arts","Martial Arts".ToSlug() ),
            GenreEntity.Create( "Space","Space".ToSlug() ),
            GenreEntity.Create( "War","War".ToSlug() ),
            GenreEntity.Create( "Police","Police".ToSlug() ),
            GenreEntity.Create( "Super Power","Super Power".ToSlug() ),
            GenreEntity.Create( "Magic","Magic".ToSlug() ),
            GenreEntity.Create( "Game","Game".ToSlug() ),
            GenreEntity.Create( "Military","Military".ToSlug() ),
            GenreEntity.Create( "Cars","Cars".ToSlug() ),
            GenreEntity.Create( "Parody","Parody".ToSlug() ),
            GenreEntity.Create( "Mature","Mature".ToSlug() ),
            GenreEntity.Create( "Cooking","Cooking".ToSlug() ),
            GenreEntity.Create( "Shounen Ai","Shounen Ai".ToSlug() ),
            GenreEntity.Create( "Shoujo Ai","Shoujo Ai".ToSlug() ),
            GenreEntity.Create( "Yaoi","Yaoi".ToSlug() ),
            GenreEntity.Create( "Yuri","Yuri".ToSlug() ),
        };

        context.Genres.AddRange(genres);
        context.SaveChanges();
    }

    private static void SeedStatuses(MangaDbContext context)
    {
        if (context.Statuses.Any()) { return; }

        var statuses = new List<StatusEntity>()
        {
            StatusEntity.Create("Ongoing","Ongoing".ToSlug()),
            StatusEntity.Create("Completed","Completed".ToSlug()),
            StatusEntity.Create("On Hold","On Hold".ToSlug()),
            StatusEntity.Create("Hiatus","Hiatus".ToSlug()),
            StatusEntity.Create("Dropped","Dropped".ToSlug()),
        };

        context.Statuses.AddRange(statuses);
        context.SaveChanges();
    }

    private static void SeedAuthors(MangaDbContext context)
    {
        if (context.Authors.Any()) { return; }

        var authors = new List<AuthorEntity>()
        {
            AuthorEntity.Create("Hajime Isayama","Hajime Isayama".ToSlug()),
            AuthorEntity.Create("Hiroshi Masuda","Hiroshi Masuda".ToSlug()),
            AuthorEntity.Create("Katsuhiro Otomo","Katsuhiro Otomo".ToSlug()),
            AuthorEntity.Create("Tatsuya Endo","Tatsuya Endo".ToSlug()),
            AuthorEntity.Create("Yukihiro Matsumoto","Yukihiro Matsumoto".ToSlug()),
            AuthorEntity.Create("Hiroshi Araki","Hiroshi Araki".ToSlug()),
            AuthorEntity.Create("Masashi Kishimoto","Masashi Kishimoto".ToSlug()),
            AuthorEntity.Create("Hirohiko Araki","Hirohiko Araki".ToSlug()),            
            AuthorEntity.Create("Hiroshi Nagai","Hiroshi Nagai".ToSlug()),
            AuthorEntity.Create("Masato Kato","Masato Kato".ToSlug()),
            AuthorEntity.Create("Hiroshi Imai","Hiroshi Imai".ToSlug()),
            AuthorEntity.Create("Akira Toriyama","Akira Toriyama".ToSlug()),
        };

        context.Authors.AddRange(authors);

        context.SaveChanges();
    }
    private static void SeedMangaTypes(MangaDbContext context)
    {
        if (context.MangaTypes.Any()) { return; }

        var mangaTypes = new List<MangaTypeEntity>()
        {
            MangaTypeEntity.Create("Manga","Manga".ToSlug()),
            MangaTypeEntity.Create("Manhwa","Manhwa".ToSlug()),
            MangaTypeEntity.Create("Manhua","Manhua".ToSlug())
        };
        context.MangaTypes.AddRange(mangaTypes);
        context.SaveChanges();
    }
    private static void SeedWebNovelTypes(MangaDbContext context)
    {
        if (context.WebNovelTypes.Any()) { return; }

        var webNovelTypes = new List<WebNovelTypeEntity>(){
            WebNovelTypeEntity.Create("WuXia","WuXia".ToSlug()),
            WebNovelTypeEntity.Create("Xianxia","Xianxia".ToSlug()),
        };

        context.WebNovelTypes.AddRange(webNovelTypes);
        context.SaveChanges();
    }

    private static void SeedImages(MangaDbContext context)
    {
        if (context.Images.Any()) { return; }

        var images = new List<ImageEntity>()
        {
            ImageEntity.Create("https://api.zscans.com/storage/76171/conversions/cbc72fbe66f77ef47aa035aca6a58a33-full.webp"),
            ImageEntity.Create("https://api.zscans.com/storage/76949/conversions/66d577365a1a39ad8be0ee2e22652ed9-full.webp"),
            ImageEntity.Create("https://api.zscans.com/storage/76648/conversions/02a263dea6a9c98e36412d3ea2e68636-full.webp"),
            ImageEntity.Create("https://api.zscans.com/storage/76645/conversions/aafd10aa43934f650bd0acbf6ed5146e-full.webp"),
            ImageEntity.Create("https://api.zscans.com/storage/77951/conversions/25face0ba019dc48773394533fe8ab09-full.webp"),
            ImageEntity.Create("https://api.zscans.com/storage/76166/conversions/6ae0e601a160413d83a02f274e1b12ca-full.webp"),
            ImageEntity.Create("https://api.zscans.com/storage/78463/conversions/5227f7e8e8b8eeff3075edb54e3c334d-full.webp"),
            ImageEntity.Create("https://api.zscans.com/storage/76173/conversions/c08d4a66edc5b093b87d93d6917b489b-full.webp"),
            ImageEntity.Create("https://api.zscans.com/storage/76189/conversions/48ab9b39ad0605103ecf87e2612ab2a4-full.webp"),
            ImageEntity.Create("https://api.zscans.com/storage/82279/conversions/e9f18f3b97c0cac44ab60ae28f98e072-full.webp"),
            ImageEntity.Create("https://api.zscans.com/storage/87388/conversions/3329ed7a1fa0320a0ed9b0208c2a3197-full.webp")
        };

        context.Images.AddRange(images);

        context.SaveChanges();
    }

    private static void SeedMangas(MangaDbContext context)
    {
        if (context.Mangas.Any()) { return; }

        var mangas = new List<MangaEntity>(){

            MangaEntity.Create(
            "One Piece",
            "One Piece".ToSlug(),
            "One Piece follows the adventurous and funny story of Monkey D. Luffy. As a boy, Luffy has always wanted to be the Pirate King. His body obtained the properties of rubber after eating a Devil Fruit. Together with a diverse crew of wannabe pirates, Luffy sets out on the ocean in an attempt to find the world’s ultimate treasure, One Piece.",
            context.Authors.AsEnumerable().AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(1, 3)).ToArray(),
            context.Images.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Statuses.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.MangaTypes.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Genres.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(2, 7)).ToArray()
            ),

            MangaEntity.Create(
            "Naruto",
            "Naruto".ToSlug(),
            "When he was a child, Naruto was isolated from its community. The people in the village treated him as he was Nine-Tails itself and don't want him. None in the village had the right to mention the Nine-Tails, in order to prevent Naruto from finding the truth. But 12 years later, he finds out the truth from ninja Mizuki, who told him.",
            context.Authors.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(1, 3)).ToArray(),
            context.Images.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Statuses.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.MangaTypes.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Genres.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(2, 7)).ToArray()
            ),

            MangaEntity.Create(
            "Bleach",
            "Bleach".ToSlug(),
            "Bleach follows the journey of Ichigo Kurosaki, a teenager who gains the powers of a Soul Reaper and battles evil spirits while unraveling the mysteries of the spirit world and his own past. With its blend of action, supernatural elements, and character-driven drama, the series captivates readers with its intricate plot and diverse cast.",
            context.Authors.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(1, 3)).ToArray(),
            context.Images.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Statuses.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.MangaTypes.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Genres.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(2, 7)).ToArray()
            ),

            MangaEntity.Create(
            "Tales of Demons and Gods",
            "Tales of Demons and Gods".ToSlug(),
            "Nie Li, one of the strongest Demon Spiritist in his past life standing at the pinnacle of the martial world , however he lost his life during the battle with Sage Emperor and the six deity ranked beast, his soul was then reborn back in time back to when he is still 13. Although he’s the weakest in his class with the lowest talent at only Red soul realm, with the aid of the vast knowledge which he accumulated in his previous life, he trained faster then anyone.",
            context.Authors.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(1, 3)).ToArray(),
            context.Images.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Statuses.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.MangaTypes.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Genres.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(2, 7)).ToArray()
            ),

            MangaEntity.Create(
            "Solo Leveling",
            "Solo Leveling".ToSlug(),
            "10 years ago, after the Gate that connected the real world with the monster world opened, some of the ordinary, everyday people received the power to hunt monsters within the Gate. They are known as Hunters. However, not all Hunters are powerful.",
            context.Authors.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(1, 3)).ToArray(),
            context.Images.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Statuses.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.MangaTypes.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Genres.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(2, 7)).ToArray()
            ),

            MangaEntity.Create(
            "Martial Peak",
            "Martial Peak".ToSlug(),
            "The journey to the martial peak is a lonely, solitary and long one.In the face of adversity,you must survive and remain unyielding.Only then can you break through and and continue on your journey to become the strongest. Sky Tower tests its disciples in the harshest ways to prepare them for this journey.One day the lowly sweeper Yang Kai managed to obtain a black book, setting him on the road to the peak of the martials world.",
            context.Authors.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(1, 3)).ToArray(),
            context.Images.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Statuses.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.MangaTypes.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Genres.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(2, 7)).ToArray()
            ),

            MangaEntity.Create(
            "Apotheosis",
            "Apotheosis".ToSlug(),
            "Luo Zheng, now a humble slave was born as the eldest son of a wealthy family. Due to his family's decline, the kidnapping of his sister by a powerful force, he can now only be stepped upon by others. However, heaven never seals off all exits. An ancient book left by his father reveals a secret divine technique, giving the reader immense power! But what is behind this power? This is a contest against fate.",
            context.Authors.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(1, 3)).ToArray(),
            context.Images.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Statuses.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.MangaTypes.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First(),
            context.Genres.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(Random.Shared.Next(2, 7)).ToArray()
            ),

        };

        context.Mangas.AddRange(mangas);
        context.SaveChanges();

    }

    private static void SeedMangaChapters(MangaDbContext context)
    {
        if (context.MangaChapters.Any()) { return; }

        var chapters = new List<MangaChapterEntity>();

        foreach (var manga in context.Mangas)
        {

            var chapterCount = Random.Shared.Next(5, 45);
            var title = $"Chapter {1}";
            var slug = title.ToSlug();
            var previousChapter = MangaChapterEntity.Create(manga, title, slug);

            chapters.Add(previousChapter);
            context.MangaChapters.Add(previousChapter);
            context.SaveChanges();

            for (int i = 1; i < chapterCount; i++)
            {
                var chapter = MangaChapterEntity.Create(manga, $"Chapter {i + 1}", $"Chapter {i + 1}".ToSlug());

                chapter.SetPreviousChapter(previousChapter);
                previousChapter.SetNextChapter(chapter);

                chapters.Add(chapter);
                previousChapter = chapter;

                context.MangaChapters.Add(chapter);
                context.SaveChanges();
            }
        }



    }

    private static void SeedMangaChapterContents(MangaDbContext context)
    {
        if (context.MangaChapterContents.Any()) { return; }

        foreach (var chapter in context.MangaChapters)
        {

            for (int i = 0; i < Random.Shared.Next(1, 5); i++)
            {

                var contentItem = context.Images.AsEnumerable().OrderBy(x => Guid.NewGuid()).Take(1).First();
                var content = MangaChapterContentEntity.Create
                (
                    chapter,
                    contentItem
                );

                context.MangaChapterContents.Add(content);
            }
        }
        context.SaveChanges();
    }


}
