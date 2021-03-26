#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0.201-buster-slim AS build
WORKDIR /app
COPY ./ ./

RUN echo 'Building VetClinicPublic container'

WORKDIR /app/VetClinicPublic/src/VetClinicPublic
RUN ls -lha .
RUN dotnet restore

RUN dotnet build "VetClinicPublic.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VetClinicPublic.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VetClinicPublic.dll"]
