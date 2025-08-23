
# MangaRead Backend Project

This is the backend for a manga reading application, currently under active development. It features a powerful web API and a flexible web crawler. This project is open-source, and I'm regularly updating the repository.

## Key Features

- **Domain-Driven Design (DDD):** A clean and scalable architecture.  
- **Flexible File Storage:** Save images locally or to any S3-compatible bucket.  
- **RESTful API:** A well-structured API for managing manga, chapters, authors, and more.  
- **Containerized:** Ready to run with Docker for easy setup and deployment.  
- **Database Migrations:** Uses Entity Framework Core for managing the database schema.  

## Project Architecture

The project follows the principles of Domain-Driven Design (DDD) to create a loosely coupled and maintainable system. The solution is divided into the following projects:

- **MangaRead.Domain:** Contains the core business logic, entities, and domain events. This is the heart of the application.  
- **MangaRead.Application:** Implements the use cases of the application, orchestrating the domain layer to perform tasks.  
- **MangaRead.Infrastructure:** Handles external concerns like database access (using EF Core), file storage (local or S3), and other third-party services.  
- **MangaRead.API:** The presentation layer, exposing the application's functionality through a RESTful API built with ASP.NET Core.  

## How to Run

You can run the project using either Docker (recommended) or the .NET CLI.

**Important! The docker file is only for local developlemt to deploy go to the deployment section at the bottom**

### Prerequisites

- Docker Desktop  
- .NET 8 SDK and Runtime  

### Running with Docker (Recommended)

Build and run the Docker image:

```bash
docker build -t manga-api .
docker run -d -p 8080:8080 manga-api
````

This will build the project, run the API.

### Running Locally with .NET CLI

This method requires the .NET 8 SDK and Runtime to be installed on your machine. The API project will automatically create and populate a local SQLite database for testing.

```bash
dotnet run --project MangaRead.API
```

### Environment Variables

* To enable Swagger UI for API testing, set `ASPNETCORE_ENVIRONMENT` to `Development`.
* To seed the database with initial data on startup, set `SHOULD_SEED` to `true`.

Both variables are set to these values by default when running in **Development**.

## Deployment

Project includes a build script called **build.sh** it will build, deploy and manage the services for the project for you. It basically does the following


1. **Build and Publish the API**
   The script first cleans any previous builds to avoid conflicts, restores all project dependencies, and compiles the API project. After building, it publishes the compiled output to a specific directory in a format suitable for running in production. This ensures the API is up-to-date and ready to execute.

2. **Database Migrations**
   Before running the API, the script applies any pending database schema changes using the ORM’s migration system. This ensures that the database structure matches what the API expects, preventing runtime errors due to missing tables or columns.

3. **System Service Setup**
   The script creates a configuration file for the system’s service manager. This allows the API to run as a background service that automatically starts on system boot, restarts if it crashes, and logs events for monitoring.

4. **Start and Reload Service**
   Once the service file is created, the script reloads the system manager to recognize the new service, enables it to start on boot, and starts the service immediately. This step ensures the API is running and properly managed by the system.

5. **Command-Line Shortcuts**
   To make managing the service easier, the script adds aliases to the user’s shell. These shortcuts allow you to quickly check the status, start, stop, or restart the API service without typing long commands.

## Database Migrations

Use the Entity Framework CLI to manage database migrations.

**Important:** Make sure the environment variable `ASPNETCORE_ENVIRONMENT` is set to `Development` before running these commands. This ensures migrations are applied to the local SQLite database file.

* Create a new migration:

```bash
dotnet ef migrations add "YourMigrationName" --project MangaRead.Infrastructure/ --startup-project MangaRead.API
```

* Apply the migration to the database:

```bash
dotnet ef database update --project MangaRead.API
```

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

* Defines the base URL where the server will run.
* `0.0.0.0` means the app listens on all network interfaces.
* Port `8080` is the active listening port.

---

### 2. **FileUpload**

```json
"FileUpload": {
  "FileOptions": [...]
}
```

* Manages **file upload settings** for the application.
* Each file type (Image, Video, Text) has:

  * **FileType** → Category of file.
  * **RealPath** → Physical location where files are stored.
  * **UrlPath** → Publicly accessible URL path for serving files.
  * **MaxFileSizeInBytes** → Maximum allowed upload size.
  * **AllowedExtensions** → Whitelisted file extensions.

**Example:**

* Images up to **10 MB** are stored in `/storage/images`.
* Videos up to **50 MB** are stored in `/storage/videos`.
* Text files (e.g., `.txt`, `.pdf`) up to **50 MB** are stored in `/storage/text`.

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

* Reserved for **object storage service configuration** (e.g., AWS S3, MinIO).
* Currently empty but should be filled with credentials and endpoint details when cloud storage is integrated.

---

### 4. **Serilog**

```json
"Serilog": {
  "Using": ["Serilog.Sinks.File", "Serilog.Sinks.Console"],
  "WriteTo": [...]
}
```

* Defines **logging configuration** using [Serilog](https://serilog.net/).
* Logs are written to:

  * **Console** → For real-time debugging and monitoring.
  * **File** → Stored in `logs/log-.txt` with:

    * **Daily rolling logs**.
    * **File size limit** handling (auto roll).

---

### 5. **ConnectionStrings**

```json
"ConnectionStrings": {
  "SqlLite": "Data Source=../manga.db",
  "MySql": ""
}
```

* Contains database connection strings.
* Currently uses **SQLite** database stored in `../manga.db`.
* Note: Sqlite is used for quick and light development if instead of Sqlite Mysql is filed it will be used instead

---

### 6. **AllowedHosts**

```json
"AllowedHosts": "*"
```

* Defines which hosts are allowed to access the application.
* `"*"` means **all hosts are allowed**.

---

## Summary

* **HostSettings** → Defines where the API runs.
* **FileUpload** → Manages storage and restrictions for different file types.
* **BucketDetails** → Placeholder for future cloud storage integration.
* **Serilog** → Handles structured logging (console + rolling file logs).
* **ConnectionStrings** → For connecting the Database.
* **AllowedHosts** → No restrictions (all hosts allowed).
---

