FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY EcoRoad-platform/EcoRoad-platform.csproj EcoRoad-platform/
RUN dotnet restore EcoRoad-platform/EcoRoad-platform.csproj

COPY EcoRoad-platform/ EcoRoad-platform/
RUN dotnet publish EcoRoad-platform/EcoRoad-platform.csproj -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
CMD ["sh", "-c", "exec dotnet EcoRoad-platform.dll --urls http://0.0.0.0:${PORT:-8080}"]
