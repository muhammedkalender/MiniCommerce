FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MiniCommerce.Processor/MiniCommerce.Processor.csproj", "MiniCommerce.Processor/"]
COPY ["MiniCommerce.Infrastructure/MiniCommerce.Infrastructure.csproj", "MiniCommerce.Infrastructure/"]
COPY ["MiniCommerce.Application/MiniCommerce.Application.csproj", "MiniCommerce.Application/"]
COPY ["MiniCommerce.Domain/MiniCommerce.Domain.csproj", "MiniCommerce.Domain/"]
RUN dotnet restore "MiniCommerce.Processor/MiniCommerce.Processor.csproj"
COPY . .
WORKDIR "/src/MiniCommerce.Processor"
RUN dotnet build "./MiniCommerce.Processor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MiniCommerce.Processor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniCommerce.Processor.dll"]
