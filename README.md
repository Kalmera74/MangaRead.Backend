# MangaRead Backend Project
This is not a finished project I am developing this for my own use and decided to opensource it. I am regularly updating this repo 

## How To Run API

Use docker to run the project. First, you need to create the image then, run the image. It'll automatically build and publish the project as well as generate the tailwind styles. You can use Docker Desktop or use the CLI.

To create the image run

```
 docker build -t manga-api .
```

To run the image in a container run

```
docker run -d -p 8080:8080 manga-api
```

Alternatively, if oyu have the dotnet SDK and Runtime installed, you can use dotnet CLI to run the individual projects. The API project will generate a sqlite database and populate it for testing purposes.

For the API project run

```
dotnet run  --project MangaRead.API
```

If you want to have Swagger make sure to set the environment variable `ASPNETCORE_ENVIRONMENT` to `Develop`. If you also want to have the database seeded also set the environment variable `SHOULD_SEED` to `true`. Both variables are set by default.

The API will listen on 8080 on all interfaces as per the appsettings.json files. If you want to change that either change the related appsettigns.json file for locally running

To create migrations use the following command

```
dotnet ef migrations add "migration_name" --projectMangaRead.Infrastructure/ --startup-projectMangaRead.API
```

enter the migration name without `""`

To update the database with the new migration run the following

```
dotnet ef database update --projectMangaRead.API
```

**_Important:_** Make sure that the environment variable `ASPNETCORE_ENVIRONMENT` is set to `Development` before running the above commands. With it set to Development it will create a local SQLite file and work on it. If it is set to anything else it will run the migration on the production DB

