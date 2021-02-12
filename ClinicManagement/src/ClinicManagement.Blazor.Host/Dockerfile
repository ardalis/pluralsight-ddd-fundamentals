#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["src/ClinicManagement.Blazor.Host/ClinicManagement.Blazor.Host.csproj", "src/ClinicManagement.Blazor.Host/"]
RUN dotnet restore "src/ClinicManagement.Blazor.Host/ClinicManagement.Blazor.Host.csproj"
COPY . .
WORKDIR "/src/src/ClinicManagement.Blazor.Host"
RUN dotnet build "ClinicManagement.Blazor.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClinicManagement.Blazor.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClinicManagement.Blazor.Host.dll"]