FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build


WORKDIR /app

COPY ["/src/", "src"]

WORKDIR /app/src/MangaLuckNeo.API

RUN dotnet restore
RUN dotnet publish "MangaRead.API.csproj" -c Release -o ../../publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime

WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 8080


ENV ASPNETCORE_ENVIRONMENT="Development"
ENV ASPNETCORE_URLS="http://0.0.0.0:8080"
ENV SHOULD_SEED="true"


ENTRYPOINT ["dotnet", "MangaRead.API.dll"]