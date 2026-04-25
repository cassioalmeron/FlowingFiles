# ========================================
# Stage 1: Build
# ========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files for restore
COPY FlowingFiles.sln .
COPY FlowingFiles.Core/FlowingFiles.Core.csproj FlowingFiles.Core/
COPY FlowingFiles.Api/FlowingFiles.Api.csproj FlowingFiles.Api/
COPY FlowingFiles.Tests/FlowingFiles.Tests.csproj FlowingFiles.Tests/

# Restore dependencies
RUN dotnet restore FlowingFiles.Api/FlowingFiles.Api.csproj

# Copy source code
COPY . .

# Build the API project
RUN dotnet build FlowingFiles.Api/FlowingFiles.Api.csproj -c Release -o /app/build

# ========================================
# Stage 2: Publish
# ========================================
FROM build AS publish
RUN dotnet publish FlowingFiles.Api/FlowingFiles.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# ========================================
# Stage 3: Runtime
# ========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# curl: health checks
# tesseract-ocr: libtesseract native library used by Tesseract NuGet on Linux
# libfontconfig1: required by SkiaSharp on Linux
# libgdiplus: required by SkiaSharp/PDFtoImage on Linux
RUN apt-get update && apt-get install -y \
    curl \
    tesseract-ocr \
    libfontconfig1 \
    libgdiplus \
    && rm -rf /var/lib/apt/lists/*

# Create data directory for SQLite with proper permissions
RUN mkdir -p /app/data && chmod 755 /app/data

# Copy published application
COPY --from=publish /app/publish .

# Expose application port
EXPOSE 5000

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5000

# Health check for container orchestration
HEALTHCHECK --interval=30s --timeout=10s --retries=3 --start-period=40s \
    CMD curl -f http://localhost:5000/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "FlowingFiles.Api.dll"]
