FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TimeTracking.API/TimeTracking.API.csproj", "TimeTracking.API/"]
COPY ["TimeTracking.Application/TimeTracking.Application.csproj", "TimeTracking.Application/"]
COPY ["TimeTracking.Contracts/TimeTracking.Contracts.csproj", "TimeTracking.Contracts/"]
RUN dotnet restore "TimeTracking.API/TimeTracking.API.csproj"
COPY . .
WORKDIR "/src/TimeTracking.API"
RUN dotnet build "./TimeTracking.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TimeTracking.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeTracking.API.dll"]
