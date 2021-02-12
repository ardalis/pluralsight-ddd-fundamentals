#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /app
COPY . .

WORKDIR /app/ClinicManagement/src/ClinicManagement.Api
RUN ls
RUN dotnet restore

RUN dotnet build "ClinicManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClinicManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClinicManagement.Api.dll"]
