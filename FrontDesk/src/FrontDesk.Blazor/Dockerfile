#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0.100-rc.1-buster-slim AS build
WORKDIR /app

COPY FrontDesk/src/FrontDesk.Blazor ./FrontDesk.Blazor
COPY FrontDesk/src/BlazorShared ./BlazorShared
RUN dotnet restore /app/FrontDesk.Blazor/FrontDesk.Blazor.csproj

WORKDIR /app/FrontDesk.Blazor
RUN dotnet build -c Release -o /bin/Release/net5.0

FROM build AS publish
RUN dotnet publish -c Release -o /bin/Release/net5.0/browser-wasm/publish

FROM nginx:alpine AS final

WORKDIR /usr/share/nginx/html

COPY --from=publish /bin/Release/net5.0/browser-wasm/publish/wwwroot /usr/local/webapp/nginx/html
COPY --from=build /bin/Release/net5.0/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80