#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8780
EXPOSE 8781

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OlMag.Identity.Api/OlMag.Identity.Api.csproj", "OlMag.Identity.Api/"]
RUN dotnet restore "./OlMag.Identity.Api/OlMag.Identity.Api.csproj"
COPY . .
WORKDIR "/src/OlMag.Identity.Api"
RUN dotnet build "./OlMag.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OlMag.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OlMag.Identity.Api.dll"]