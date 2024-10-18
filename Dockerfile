FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["MangaRead.API/MangaRead.API.csproj", "MangaRead.API/"]
RUN dotnet restore "./MangaRead.API/MangaRead.API.csproj"

COPY . .

WORKDIR "/src/MangaRead.API"

RUN dotnet build "./MangaRead.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./MangaRead.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

EXPOSE 8080

WORKDIR /app

COPY --from=publish /app/publish .


ENV ASPNETCORE_ENVIRONMENT=Development
ENV SHOULD_SEED=true


ENTRYPOINT ["dotnet", "MangaRead.API.dll"]
