FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src

COPY . ./
WORKDIR /src/Admin.Web
RUN dotnet publish "Admin.Web.csproj" -c Release -o /app

FROM build-env
ENV PORT 8080
ENV FOLDER_PATH admin
WORKDIR /app
#COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "Admin.Web.dll"]