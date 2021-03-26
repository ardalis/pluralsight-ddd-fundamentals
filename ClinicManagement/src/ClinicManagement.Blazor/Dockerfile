#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# From solution root:
# docker build -t cm --progress=plain --no-cache -f .\ClinicManagement\src\ClinicManagement.Blazor.Host\Dockerfile .
FROM mcr.microsoft.com/dotnet/sdk:5.0.201-buster-slim AS build
WORKDIR /app

COPY ClinicManagement/src/ClinicManagement.Blazor ./ClinicManagement.Blazor
COPY ClinicManagement/src/BlazorShared ./BlazorShared

RUN ls -lha

RUN dotnet restore /app/ClinicManagement.Blazor/ClinicManagement.Blazor.csproj

WORKDIR /app/ClinicManagement.Blazor
RUN dotnet build -c Release -o /bin/Release/net5.0

FROM build AS publish
RUN dotnet publish -c Release -o /bin/Release/net5.0/browser-wasm/publish

FROM nginx:alpine AS final

WORKDIR /usr/share/nginx/html

COPY --from=publish /bin/Release/net5.0/browser-wasm/publish/wwwroot /usr/local/webapp/nginx/html
COPY --from=build /bin/Release/net5.0/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80