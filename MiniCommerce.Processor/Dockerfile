# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

# Build and test stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy .csproj files
COPY ["MiniCommerce.Processor/MiniCommerce.Processor.csproj", "MiniCommerce.Processor/"]
COPY ["MiniCommerce.Infrastructure/MiniCommerce.Infrastructure.csproj", "MiniCommerce.Infrastructure/"]
COPY ["MiniCommerce.Application/MiniCommerce.Application.csproj", "MiniCommerce.Application/"]
COPY ["MiniCommerce.Domain/MiniCommerce.Domain.csproj", "MiniCommerce.Domain/"]
COPY ["MiniCommerce.Processor.Tests/MiniCommerce.Processor.Tests.csproj", "MiniCommerce.Processor.Tests/"]

# Restore dependencies
RUN dotnet restore "MiniCommerce.Processor/MiniCommerce.Processor.csproj"

# Copy the rest of the code
COPY . .

# Run unit tests BEFORE build
RUN dotnet test MiniCommerce.Processor.Tests/MiniCommerce.Processor.Tests.csproj \
    --configuration $BUILD_CONFIGURATION \
    --no-build \
    --verbosity normal

# Build main project
WORKDIR "/src/MiniCommerce.Processor"
RUN dotnet build "./MiniCommerce.Processor.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MiniCommerce.Processor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniCommerce.Processor.dll"]
