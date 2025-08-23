using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaLuckNeo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Url = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MangaType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaType", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserName = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WebNovelChapterContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Body = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaTitle = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaDescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelChapterContent", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WebNovelType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelType", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StarCount = table.Column<float>(type: "float", maxLength: 3, nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UsersId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WebNovel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    SeasonCount = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<float>(type: "float", nullable: false),
                    MetaTitle = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaDescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CoverImageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StatusId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MangaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovel", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                    table.ForeignKey(
                        name: "FK_WebNovel_Image_CoverImageId",
                        column: x => x.CoverImageId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebNovel_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebNovel_WebNovelType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "WebNovelType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Manga",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    SeasonCount = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<float>(type: "float", nullable: false),
                    IsPublished = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MetaTitle = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaDescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CoverImageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StatusId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WebNovelId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manga", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                    table.ForeignKey(
                        name: "FK_Manga_Image_CoverImageId",
                        column: x => x.CoverImageId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manga_MangaType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MangaType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manga_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manga_WebNovel_WebNovelId",
                        column: x => x.WebNovelId,
                        principalTable: "WebNovel",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserBookmarkedWebNovels",
                columns: table => new
                {
                    BookMarkedWebNovelsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBookmarkedWebNovels", x => new { x.BookMarkedWebNovelsId, x.UserEntityId });
                    table.ForeignKey(
                        name: "FK_UserBookmarkedWebNovels_Users_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBookmarkedWebNovels_WebNovel_BookMarkedWebNovelsId",
                        column: x => x.BookMarkedWebNovelsId,
                        principalTable: "WebNovel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WebNovelAuthors",
                columns: table => new
                {
                    AuthorsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WebNovelEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelAuthors", x => new { x.AuthorsId, x.WebNovelEntityId });
                    table.ForeignKey(
                        name: "FK_WebNovelAuthors_Author_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebNovelAuthors_WebNovel_WebNovelEntityId",
                        column: x => x.WebNovelEntityId,
                        principalTable: "WebNovel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WebNovelChapter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreviousChapterId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    NextChapterId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ContentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    WebNovelId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelChapter", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                    table.ForeignKey(
                        name: "FK_WebNovelChapter_WebNovelChapterContent_ContentId",
                        column: x => x.ContentId,
                        principalTable: "WebNovelChapterContent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WebNovelChapter_WebNovelChapter_NextChapterId",
                        column: x => x.NextChapterId,
                        principalTable: "WebNovelChapter",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WebNovelChapter_WebNovelChapter_PreviousChapterId",
                        column: x => x.PreviousChapterId,
                        principalTable: "WebNovelChapter",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WebNovelChapter_WebNovel_WebNovelId",
                        column: x => x.WebNovelId,
                        principalTable: "WebNovel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WebNovelGenres",
                columns: table => new
                {
                    GenresId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WebNovelEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelGenres", x => new { x.GenresId, x.WebNovelEntityId });
                    table.ForeignKey(
                        name: "FK_WebNovelGenres_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebNovelGenres_WebNovel_WebNovelEntityId",
                        column: x => x.WebNovelEntityId,
                        principalTable: "WebNovel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WebNovelRatings",
                columns: table => new
                {
                    RatingsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WebNovelEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelRatings", x => new { x.RatingsId, x.WebNovelEntityId });
                    table.ForeignKey(
                        name: "FK_WebNovelRatings_Ratings_RatingsId",
                        column: x => x.RatingsId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebNovelRatings_WebNovel_WebNovelEntityId",
                        column: x => x.WebNovelEntityId,
                        principalTable: "WebNovel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MangaAuthors",
                columns: table => new
                {
                    AuthorsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MangaEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaAuthors", x => new { x.AuthorsId, x.MangaEntityId });
                    table.ForeignKey(
                        name: "FK_MangaAuthors_Author_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaAuthors_Manga_MangaEntityId",
                        column: x => x.MangaEntityId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MangaChapter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaTitle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPublished = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PreviousChapterId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    NextChapterId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MangaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaChapter", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                    table.ForeignKey(
                        name: "FK_MangaChapter_MangaChapter_NextChapterId",
                        column: x => x.NextChapterId,
                        principalTable: "MangaChapter",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MangaChapter_MangaChapter_PreviousChapterId",
                        column: x => x.PreviousChapterId,
                        principalTable: "MangaChapter",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MangaChapter_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MangaGenres",
                columns: table => new
                {
                    GenresId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MangaEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaGenres", x => new { x.GenresId, x.MangaEntityId });
                    table.ForeignKey(
                        name: "FK_MangaGenres_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaGenres_Manga_MangaEntityId",
                        column: x => x.MangaEntityId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MangaRatings",
                columns: table => new
                {
                    MangaEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RatingsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaRatings", x => new { x.MangaEntityId, x.RatingsId });
                    table.ForeignKey(
                        name: "FK_MangaRatings_Manga_MangaEntityId",
                        column: x => x.MangaEntityId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaRatings_Ratings_RatingsId",
                        column: x => x.RatingsId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserBookmarkedMangas",
                columns: table => new
                {
                    BookMarkedMangasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBookmarkedMangas", x => new { x.BookMarkedMangasId, x.UserEntityId });
                    table.ForeignKey(
                        name: "FK_UserBookmarkedMangas_Manga_BookMarkedMangasId",
                        column: x => x.BookMarkedMangasId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBookmarkedMangas_Users_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MangaChapterContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ChapterId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaChapterContent", x => x.Id)
                        .Annotation("MySql:IndexPrefixLength", new[] { 36 });
                    table.ForeignKey(
                        name: "FK_MangaChapterContent_Image_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaChapterContent_MangaChapter_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "MangaChapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Author_Slug",
                table: "Author",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Slug",
                table: "Genres",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manga_CoverImageId",
                table: "Manga",
                column: "CoverImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Manga_Slug",
                table: "Manga",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manga_StatusId",
                table: "Manga",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Manga_TypeId",
                table: "Manga",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Manga_WebNovelId",
                table: "Manga",
                column: "WebNovelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MangaAuthors_MangaEntityId",
                table: "MangaAuthors",
                column: "MangaEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaChapter_MangaId",
                table: "MangaChapter",
                column: "MangaId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaChapter_NextChapterId",
                table: "MangaChapter",
                column: "NextChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaChapter_PreviousChapterId",
                table: "MangaChapter",
                column: "PreviousChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaChapterContent_ChapterId",
                table: "MangaChapterContent",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaChapterContent_ItemId",
                table: "MangaChapterContent",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaGenres_MangaEntityId",
                table: "MangaGenres",
                column: "MangaEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaRatings_RatingsId",
                table: "MangaRatings",
                column: "RatingsId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaType_Slug",
                table: "MangaType",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Slug",
                table: "Statuses",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBookmarkedMangas_UserEntityId",
                table: "UserBookmarkedMangas",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookmarkedWebNovels_UserEntityId",
                table: "UserBookmarkedWebNovels",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UsersId",
                table: "UserRoles",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovel_CoverImageId",
                table: "WebNovel",
                column: "CoverImageId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovel_Slug",
                table: "WebNovel",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebNovel_StatusId",
                table: "WebNovel",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovel_TypeId",
                table: "WebNovel",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelAuthors_WebNovelEntityId",
                table: "WebNovelAuthors",
                column: "WebNovelEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapter_ContentId",
                table: "WebNovelChapter",
                column: "ContentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapter_NextChapterId",
                table: "WebNovelChapter",
                column: "NextChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapter_PreviousChapterId",
                table: "WebNovelChapter",
                column: "PreviousChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapter_WebNovelId",
                table: "WebNovelChapter",
                column: "WebNovelId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelGenres_WebNovelEntityId",
                table: "WebNovelGenres",
                column: "WebNovelEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelRatings_WebNovelEntityId",
                table: "WebNovelRatings",
                column: "WebNovelEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelType_Slug",
                table: "WebNovelType",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangaAuthors");

            migrationBuilder.DropTable(
                name: "MangaChapterContent");

            migrationBuilder.DropTable(
                name: "MangaGenres");

            migrationBuilder.DropTable(
                name: "MangaRatings");

            migrationBuilder.DropTable(
                name: "UserBookmarkedMangas");

            migrationBuilder.DropTable(
                name: "UserBookmarkedWebNovels");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "WebNovelAuthors");

            migrationBuilder.DropTable(
                name: "WebNovelChapter");

            migrationBuilder.DropTable(
                name: "WebNovelGenres");

            migrationBuilder.DropTable(
                name: "WebNovelRatings");

            migrationBuilder.DropTable(
                name: "MangaChapter");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "WebNovelChapterContent");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Manga");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MangaType");

            migrationBuilder.DropTable(
                name: "WebNovel");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "WebNovelType");
        }
    }
}
