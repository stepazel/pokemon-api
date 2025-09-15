FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["YolkStudio.Pokemon.Api/YolkStudio.Pokemon.Api.csproj", "YolkStudio.Pokemon.Api/"]
COPY ["YolkStudio.Pokemon.Core/YolkStudio.Pokemon.Core.csproj", "YolkStudio.Pokemon.Core/"]
COPY ["YolkStudio.Pokemon.Infrastructure/YolkStudio.Pokemon.Infrastructure.csproj", "YolkStudio.Pokemon.Infrastructure/"]
RUN dotnet restore "YolkStudio.Pokemon.Api/YolkStudio.Pokemon.Api.csproj"
COPY . .
WORKDIR "/src/YolkStudio.Pokemon.Api"
RUN dotnet build "./YolkStudio.Pokemon.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./YolkStudio.Pokemon.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YolkStudio.Pokemon.Api.dll"]
