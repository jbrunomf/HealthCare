FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG APP_UID=1000
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HealthCare.Web/HealthCare.Web.csproj", "HealthCare.Web/"]
COPY ["HealthCare.Business/HealthCare.Business.csproj", "HealthCare.Business/"]
COPY ["HealthCare.Data/HealthCare.Data.csproj", "HealthCare.Data/"]
RUN dotnet restore "HealthCare.Web/HealthCare.Web.csproj"
COPY . .
WORKDIR "/src/HealthCare.Web"
RUN dotnet build "HealthCare.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HealthCare.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthCare.Web.dll"]
