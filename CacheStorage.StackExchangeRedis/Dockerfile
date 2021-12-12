#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CacheStorage.StackExchangeRedis/CacheStorage.StackExchangeRedis.csproj", "CacheStorage.StackExchangeRedis/"]
RUN dotnet restore "CacheStorage.StackExchangeRedis/CacheStorage.StackExchangeRedis.csproj"
COPY . .
WORKDIR "/src/CacheStorage.StackExchangeRedis"
RUN dotnet build "CacheStorage.StackExchangeRedis.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CacheStorage.StackExchangeRedis.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CacheStorage.StackExchangeRedis.dll"]