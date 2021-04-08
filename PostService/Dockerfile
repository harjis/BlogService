FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# So that we can run migrations
RUN dotnet tool install --global dotnet-ef --version 5.0.4
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /source

# copy csproj and restore as distinct layers
COPY PostService.csproj .

# copy and publish app and libraries
COPY . .
RUN dotnet build
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "PostService.dll"]