FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LogKeeperPg/LogKeeperPg.csproj", "LogKeeperPg/"]
RUN dotnet restore "LogKeeperPg/LogKeeperPg.csproj"
COPY . .
WORKDIR "/src/LogKeeperPg"
RUN dotnet build "LogKeeperPg.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LogKeeperPg.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogKeeperPg.dll"]
