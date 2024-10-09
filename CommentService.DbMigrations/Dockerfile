FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CommentService.DbMigrations/CommentService.DbMigrations.csproj", "CommentService.DbMigrations/"]
RUN dotnet restore "CommentService.DbMigrations/CommentService.DbMigrations.csproj"
COPY . .
WORKDIR "/src/CommentService.DbMigrations"
RUN dotnet build "CommentService.DbMigrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommentService.DbMigrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommentService.DbMigrations.dll"]
