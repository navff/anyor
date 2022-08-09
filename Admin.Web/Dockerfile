FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update
RUN apt-get install -y libc6-dev 
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Admin.Web/Admin.Web.csproj"
COPY . .
WORKDIR "/src/Admin.Web"
RUN dotnet build "Admin.Web.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Admin.Web.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV PORT 9999
ENV FOLDER_PATH admin
ENV ASPNETCORE_ENVIRONMENT Development
# EXPOSE 9999
# ENV YDB_ENDPOINT grpcs://ydb.serverless.yandexcloud.net:2135
# ENV YDB_ADDRESS 
# ENV YA_PRIVATE_KEY 
# ENV YA_KEY_ID 
# ENV YA_SERVICE_ACCOUNT_ID 
ENV FOLDER_PATH admin
ENTRYPOINT ["dotnet", "Admin.Web.dll"]