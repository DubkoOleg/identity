#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8781

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OlMag.Identity.Api/OlMag.Identity.Api.csproj", "."]
RUN dotnet restore "OlMag.Identity.Api.csproj"

COPY ["OlMag.Identity.Api/", "."]
WORKDIR /src
RUN dotnet build "OlMag.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OlMag.Identity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OlMag.Identity.Api.dll"]
