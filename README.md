# Manga Read Backend

MangaRead Backend is a scalable, DDD-based backend for a manga and WebNovel reading application.  
It provides a RESTful API, flexible file storage (local or S3), database migrations, and integrates seamlessly with the [MangaRead Crawler](https://github.com/Kalmera74/MangaRead.Crawler).

## Key Features

- **Domain-Driven Design (DDD):** Clean, maintainable architecture.
- **RESTful API:** Manage mangas, chapters, authors, and more.
- **Flexible File Storage:** Supports local and S3-compatible storage.
- **Database Migrations:** Uses Entity Framework Core.
- **Containerized:** Ready to run with Docker.
- **Logging:** Structured logging with Serilog.
- **Deployment Automation:** `build.sh` script for building, migrating, and running as a system service.

---

## Project Architecture

The project follows the principles of Domain-Driven Design (DDD) to create a loosely coupled and maintainable system. The solution is divided into the following projects:

- **MangaRead.Domain:** Contains the core business logic, entities, and domain events. This is the heart of the application.
- **MangaRead.Application:** Implements the use cases of the application, orchestrating the domain layer to perform tasks.
- **MangaRead.Infrastructure:** Handles external concerns like database access (using EF Core), file storage (local or S3), and other third-party services.
- **MangaRead.API:** The presentation layer, exposing the application's functionality through a RESTful API built with ASP.NET Core.

---

## Quick Start

You can run the project using either Docker (recommended) or the .NET CLI.

**Important! The docker file is only for local development. To deploy go to the deployment section at the bottom**

### Prerequisites

- Docker Desktop  
  or
- .NET 8 SDK and Runtime

First clone the project then you can run it with either docker or trough .NET CLI

```bash
git clone https://github.com/Kalmera74/MangaRead.Backend
cd MangaRead.Backend
```
The API will listen on `8080` on all interfaces as per the `appsettings.json` file. If you want to change that either change the related appsettings.json file for locally running or set the environment variable `ASPNETCORE_URLS` to what you need in Docker


### Running with Docker (Recommended)

Build and run the Docker image:

```bash
docker build -t manga-api .
docker run -d -p 8080:8080 manga-api
```

This will build the project, run the API.

### Running Locally with .NET CLI

This method requires the .NET 8 SDK and Runtime to be installed on your machine. The API project will automatically create and populate a local SQLite database for testing.

```bash
dotnet run --project src/MangaRead.API
```
To create migrations use the following command 
```bash
dotnet ef migrations add "migration_name" --project src/MangaRead.Infrastructure/ --startup-project src/MangaRead.API
```
 enter the migration name without `""`

To update the database with the new migration run the following
```bash
dotnet ef database update --project src/MangaRead.API
```
### Environment Variables

- To enable Swagger UI for API testing, set `ASPNETCORE_ENVIRONMENT` to `Development`.
- To seed the database with initial data on startup, set `SHOULD_SEED` to `true`.

Both variables are set to these values by default when running in **Development**.

## Deployment

This project includes a `build.sh` script that automates deployment:

1. Builds and publishes the API
2. Applies database migrations
3. Configures as a system service (auto-restart, logging)
4. Starts/reloads the service
5. Adds CLI shortcuts for management



## Configuration

The API's behavior is configured in **`MangaRead.API/appsettings.json`**. You can have appsettings.json per environemnt and they will be automatically selected and used. eg. appsettings.Development.json. This is a base starting point settings and should be used as reference

```json
{
  "HostSettings": {
    "Url": "http://0.0.0.0:8080"
  },
  "FileUpload": {
    "FileOptions": [
      {
        "FileType": "Image",
        "RealPath": "../MangaRead.Frontend/wwwroot/storage/images",
        "UrlPath": "/storage/images",
        "MaxFileSizeInBytes": 10485760,
        "AllowedExtensions": ["jpg", "jpeg", "png", "gif", "webp"]
      },
      {
        "FileType": "Video",
        "RealPath": "../MangaRead.Frontend/wwwroot/storage/videos",
        "UrlPath": "/storage/images",
        "MaxFileSizeInBytes": 52428800,
        "AllowedExtensions": [".mp4", ".mkv"]
      },
      {
        "FileType": "Text",
        "RealPath": "../MangaRead.Frontend/wwwroot/storage/text",
        "UrlPath": "/storage/images",
        "MaxFileSizeInBytes": 52428800,
        "AllowedExtensions": [".txt", ".pdf"]
      }
    ]
  },
  "BucketDetails": {
    "BucketName": "",
    "SecretKey": "",
    "AccessKey": "",
    "ServiceURL": ""
  },
  "Serilog": {
    "Using": ["Serilog.Sinks.File", "Serilog.Sinks.Console"],
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "rollOnFileSizeLimit": true
        }
      }
    ]
  },
  "ConnectionStrings": {
    "SqlLite": "Data Source=../manga.db"
  },
  "AllowedHosts": "*"
}
```

---

## Sections

### 1. **HostSettings**

```json
"HostSettings": {
  "Url": "http://0.0.0.0:8080"
}
```

- Defines the base URL where the server will run.
- `0.0.0.0` means the app listens on all network interfaces.
- Port `8080` is the active listening port.

---

### 2. **FileUpload**

```json
"FileUpload": {
  "FileOptions": [...]
}
```

- Manages **file upload settings** for the application.
- Each file type (Image, Video, Text) has:

  - **FileType** → Category of file.
  - **RealPath** → Physical location where files are stored.
  - **UrlPath** → Publicly accessible URL path for serving files.
  - **MaxFileSizeInBytes** → Maximum allowed upload size.
  - **AllowedExtensions** → Whitelisted file extensions.

**Example:**

- Images up to **10 MB** are stored in `/storage/images`.
- Videos up to **50 MB** are stored in `/storage/videos`.
- Text files (e.g., `.txt`, `.pdf`) up to **50 MB** are stored in `/storage/text`.

---

### 3. **BucketDetails**

```json
"BucketDetails": {
  "BucketName": "",
  "SecretKey": "",
  "AccessKey": "",
  "ServiceURL": ""
}
```

- Reserved for **object storage service configuration** (e.g., AWS S3, MinIO).
- Currently empty but should be filled with credentials and endpoint details when cloud storage is integrated.

---

### 4. **Serilog**

```json
"Serilog": {
  "Using": ["Serilog.Sinks.File", "Serilog.Sinks.Console"],
  "WriteTo": [...]
}
```

- Defines **logging configuration** using [Serilog](https://serilog.net/).
- Logs are written to:

  - **Console** → For real-time debugging and monitoring.
  - **File** → Stored in `logs/log-.txt` with:

    - **Daily rolling logs**.
    - **File size limit** handling (auto roll).

---

### 5. **ConnectionStrings**

```json
"ConnectionStrings": {
  "SqlLite": "Data Source=../manga.db",
  "MySql": ""
}
```

- Contains database connection strings.
- Currently uses **SQLite** database stored in `../manga.db`.
- Note: Sqlite is used for quick and light development if instead of Sqlite Mysql is filed it will be used instead

---

### 6. **AllowedHosts**

```json
"AllowedHosts": "*"
```

- Defines which hosts are allowed to access the application.
- `"*"` means **all hosts are allowed**.

---

## Summary

- **HostSettings** → Defines where the API runs.
- **FileUpload** → Manages storage and restrictions for different file types.
- **BucketDetails** → Placeholder for future cloud storage integration.
- **Serilog** → Handles structured logging (console + rolling file logs).
- **ConnectionStrings** → For connecting the Database.
- **AllowedHosts** → No restrictions (all hosts allowed).

---
