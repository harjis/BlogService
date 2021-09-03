FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["PostService.DbMigrations/PostService.DbMigrations.csproj", "PostService.DbMigrations/"]
RUN dotnet restore "PostService.DbMigrations/PostService.DbMigrations.csproj"
COPY . .
WORKDIR "/src/PostService.DbMigrations"
RUN dotnet build "PostService.DbMigrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PostService.DbMigrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PostService.DbMigrations.dll"]
