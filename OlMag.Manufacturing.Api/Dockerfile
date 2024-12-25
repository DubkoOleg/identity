#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8040
EXPOSE 8041

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OlMag.Manufacturing.Api/OlMag.Manufacturing.Api.csproj", "OlMag.Manufacturing.Api/"]
RUN dotnet restore "./OlMag.Manufacturing.Api/./OlMag.Manufacturing.Api.csproj"
COPY . .
WORKDIR "/src/OlMag.Manufacturing.Api"
RUN dotnet build "./OlMag.Manufacturing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OlMag.Manufacturing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OlMag.Manufacturing.Api.dll"]