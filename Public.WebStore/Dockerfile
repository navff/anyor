FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
ENV TOKEN ${AUTH_TOKEN_GITHUB}
RUN echo $TOKEN

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
ENV TOKEN ${AUTH_TOKEN_GITHUB}
RUN dotnet nuget add source https://nuget.pkg.github.com/navff/index.json --name="github" --username navff --valid-authentication-types basic --store-password-in-clear-text --password ${TOKEN}
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
