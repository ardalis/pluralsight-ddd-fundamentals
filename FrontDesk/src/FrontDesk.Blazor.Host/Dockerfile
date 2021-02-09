#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/FrontDesk.Blazor.Host/FrontDesk.Blazor.Host.csproj", "src/FrontDesk.Blazor.Host/"]
RUN dotnet restore "src/FrontDesk.Blazor.Host/FrontDesk.Blazor.Host.csproj"
COPY . .
WORKDIR "/src/src/FrontDesk.Blazor.Host"
RUN dotnet build "FrontDesk.Blazor.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FrontDesk.Blazor.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrontDesk.Blazor.Host.dll"]