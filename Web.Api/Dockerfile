FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update
RUN apt-get install -y libc6-dev 
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Web.Api/Web.Api.csproj"
COPY . .
WORKDIR "/src/Web.Api"
RUN dotnet build "Web.Api.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Web.Api.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV PORT 9999
ENV FOLDER_PATH admin
ENV ASPNETCORE_ENVIRONMENT Development
ENV FOLDER_PATH admin
ENTRYPOINT ["dotnet", "Web.Api.dll"]
