using MangaRead.Application.Repositories.Author;
using MangaRead.Application.Repositories.Genre;
using MangaRead.Application.Repositories.Rating;
using MangaRead.Application.Repositories.Readables.Manga;
using MangaRead.Application.Repositories.Readables.Manga.Chapter;
using MangaRead.Application.Repositories.Readables.Manga.Chapter.Content;
using MangaRead.Application.Repositories.Readables.Manga.Type;
using MangaRead.Application.Repositories.Readables.WebNovel;
using MangaRead.Application.Repositories.Readables.WebNovel.Chapter;
using MangaRead.Application.Repositories.Readables.WebNovel.Chapter.Content;
using MangaRead.Application.Repositories.Readables.WebNovel.Type;
using MangaRead.Application.Repositories.Status;
using MangaRead.Application.Repositories.System.Image;
using MangaRead.Application.Repositories.System.User;
using MangaRead.Application.UnitOfWork;
using MangaRead.Infrastructure.DbContexts;
using MangaRead.Infrastructure.Repositories.Author;
using MangaRead.Infrastructure.Repositories.Genre;
using MangaRead.Infrastructure.Repositories.Rating;
using MangaRead.Infrastructure.Repositories.Readables.Manga;
using MangaRead.Infrastructure.Repositories.Readables.Manga.Chapter;
using MangaRead.Infrastructure.Repositories.Readables.Manga.Chapter.Content;
using MangaRead.Infrastructure.Repositories.Readables.Manga.Type;
using MangaRead.Infrastructure.Repositories.Readables.WebNovel;
using MangaRead.Infrastructure.Repositories.Readables.WebNovel.Chapter;
using MangaRead.Infrastructure.Repositories.Readables.WebNovel.Chapter.Content;
using MangaRead.Infrastructure.Repositories.Readables.WebNovel.Type;
using MangaRead.Infrastructure.Repositories.Status;
using MangaRead.Infrastructure.Repositories.System.Image;
using MangaRead.Infrastructure.Repositories.System.User;
using MangaRead.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MangaRead.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddDbContext<MangaDbContext>(opt =>
            {
                if (environment.IsDevelopment())
                {
                    var connectionString = configuration.GetConnectionString("SqlLite");
                    opt.UseSqlite(connectionString);
                }
                else
                {
                    var connectionString = configuration.GetConnectionString("Mysql");
                    opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                }
            }, ServiceLifetime.Scoped);


            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IMangaRepository, MangaRepository>();
            services.AddScoped<IMangaTypeRepository, MangaTypeRepository>();
            services.AddScoped<IMangaChapterContentRepository, MangaChapterContentRepository>();
            services.AddScoped<IMangaChapterRepository, MangaChapterRepository>();
            services.AddScoped<IWebNovelRepository, WebNovelRepository>();
            services.AddScoped<IWebNovelTypeRepository, WebNovelTypeRepository>();
            services.AddScoped<IWebNovelChapterRepository, WebNovelChapterRepository>();
            services.AddScoped<IWebNovelChapterContentRepository, WebNovelChapterContentRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();


            services.AddTransient<IUnitOfWork, MangaLuckUnitOfWork>();

            return services;
        }
    }
}
