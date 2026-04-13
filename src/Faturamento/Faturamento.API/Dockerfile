FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Faturamento/Faturamento.API/Faturamento.API.csproj", "src/Faturamento/Faturamento.API/"]
COPY ["src/Faturamento/Faturamento.Domain/Faturamento.Domain.csproj", "src/Faturamento/Faturamento.Domain/"]
COPY ["src/Faturamento/Faturamento.Infrastructure/Faturamento.Infrastructure.csproj", "src/Faturamento/Faturamento.Infrastructure/"]
COPY ["src/Faturamento/Faturamento.Application/Faturamento.Application.csproj", "src/Faturamento/Faturamento.Application/"]
RUN dotnet restore "src/Faturamento/Faturamento.API/Faturamento.API.csproj"
COPY . .
WORKDIR "/src/src/Faturamento/Faturamento.API"
RUN dotnet build "./Faturamento.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Faturamento.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Faturamento.API.dll"]
