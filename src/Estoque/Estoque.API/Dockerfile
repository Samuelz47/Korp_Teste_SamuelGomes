FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Estoque/Estoque.API/Estoque.API.csproj", "src/Estoque/Estoque.API/"]
COPY ["src/Estoque/Estoque.Domain/Estoque.Domain.csproj", "src/Estoque/Estoque.Domain/"]
COPY ["src/Estoque/Estoque.Infrastructure/Estoque.Infrastructure.csproj", "src/Estoque/Estoque.Infrastructure/"]
COPY ["src/Estoque/Estoque.Application/Estoque.Application.csproj", "src/Estoque/Estoque.Application/"]
RUN dotnet restore "src/Estoque/Estoque.API/Estoque.API.csproj"
COPY . .
WORKDIR "/src/src/Estoque/Estoque.API"
RUN dotnet build "./Estoque.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Estoque.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN mkdir -p /app/data && chmod 777 /app/data
USER $APP_UID
ENTRYPOINT ["dotnet", "Estoque.API.dll"]
