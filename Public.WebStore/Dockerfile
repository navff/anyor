FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet nuget add source --username navff --password ${AUTH_TOKEN_GITHUB} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/navff/index.json"
RUN dotnet restore "Public.WebStore/Public.WebStore.csproj"
COPY . .
WORKDIR "/src/Public.WebStore"
RUN dotnet build "Public.WebStore.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Public.WebStore.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Public.WebStore.dll"]
