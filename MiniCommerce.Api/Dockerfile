# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build and test stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files
COPY ["MiniCommerce.Api/MiniCommerce.Api.csproj", "MiniCommerce.Api/"]
COPY ["MiniCommerce.Application/MiniCommerce.Application.csproj", "MiniCommerce.Application/"]
COPY ["MiniCommerce.Domain/MiniCommerce.Domain.csproj", "MiniCommerce.Domain/"]
COPY ["MiniCommerce.Infrastructure/MiniCommerce.Infrastructure.csproj", "MiniCommerce.Infrastructure/"]
COPY ["MiniCommerce.Api.Tests/MiniCommerce.Api.Tests.csproj", "MiniCommerce.Api.Tests/"]

# Restore
RUN dotnet restore "MiniCommerce.Api/MiniCommerce.Api.csproj"

# Copy all files
COPY . .

# ⬇️ Run unit tests before building the final output
RUN dotnet test MiniCommerce.Api.Tests/MiniCommerce.Api.Tests.csproj --configuration $BUILD_CONFIGURATION --no-build --verbosity normal

# Build the app
WORKDIR "/src/MiniCommerce.Api"
RUN dotnet build "./MiniCommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MiniCommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniCommerce.Api.dll"]
