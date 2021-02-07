#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0.0-rc.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0.100-rc.1-buster-slim AS build
WORKDIR /app
COPY . .

WORKDIR /app/FrontDesk/src/FrontDesk.Api
RUN dotnet restore

RUN dotnet build "FrontDesk.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FrontDesk.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrontDesk.Api.dll"]