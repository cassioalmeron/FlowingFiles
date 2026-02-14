# Docker Setup Reference Guide

## CRITICAL: .env File Requirement

**ALL Docker configurations require a .env file to be configured before running containers.**

### Steps:
1. Generate `.env.example` with all required variables
2. User copies: `cp .env.example .env`
3. User configures all variables in `.env`
4. Run: `docker-compose up --build`

### Important Rules:
- **NO default values** in docker-compose.yml (no `${VAR:-default}` syntax)
- Use `${VARIABLE_NAME}` to require the variable from .env
- Containers will fail to start if .env is missing or incomplete
- Document every variable with comments in .env.example

---

## .env.example Templates

### Full-Stack (.NET/Python Backend + React Frontend)

```bash
# === Application Configuration ===
APP_NAME=MyApplication

# === Port Configuration ===
BACKEND_PORT=5000
FRONTEND_PORT=80

# === Backend Configuration ===
# Environment: Development, Staging, Production
ASPNETCORE_ENVIRONMENT=Production

# === Database Configuration ===
# Provider: SQLite or PostgreSQL
DB_PROVIDER=SQLite
DB_PATH=/app/data/app.db

# PostgreSQL Configuration (if DB_PROVIDER=PostgreSQL)
DB_HOST=db
DB_PORT=5432
DB_NAME=myapp
DB_USER=postgres
DB_PASSWORD=change-this-secure-password

# === JWT Authentication ===
# IMPORTANT: Change this to a strong random key in production
JWT_KEY=change-this-to-a-secure-random-key-at-least-32-characters-long
JWT_ISSUER=MyApplication
JWT_AUDIENCE=MyApplication
JWT_EXPIRY_MINUTES=60

# === Frontend Build Configuration ===
# API URL that frontend will use
VITE_API_URL=http://localhost:5000
VITE_APP_NAME=My Application

# === Docker Configuration ===
STDIN_OPEN=false
TTY=false
```

### Backend Only (.NET)

```bash
# === Application Configuration ===
APP_NAME=MyBackend

# === Port Configuration ===
BACKEND_PORT=5000

# === Backend Configuration ===
ASPNETCORE_ENVIRONMENT=Production

# === Database Configuration ===
DB_PROVIDER=SQLite
DB_PATH=/app/data/app.db

# === JWT Authentication ===
JWT_KEY=change-this-to-a-secure-random-key-at-least-32-characters-long
JWT_ISSUER=MyBackend
JWT_AUDIENCE=MyBackend
JWT_EXPIRY_MINUTES=60
```

### Backend Only (Python)

```bash
# === Application Configuration ===
APP_NAME=MyPythonBackend

# === Port Configuration ===
BACKEND_PORT=5000

# === Backend Configuration ===
PYTHON_ENV=production
DEBUG=false

# === Database Configuration ===
DATABASE_URL=postgresql://user:password@db:5432/myapp
# Or for SQLite:
# DATABASE_URL=sqlite:///./app.db

# === Secret Keys ===
SECRET_KEY=change-this-to-a-secure-random-secret-key
API_KEY=change-this-api-key
```

---

## .NET Backend Template

### Dockerfile Pattern

```dockerfile
# ========================================
# Stage 1: Build
# ========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files for restore
COPY *.sln .
COPY ProjectName.Api/ProjectName.Api.csproj ProjectName.Api/
COPY ProjectName.Core/ProjectName.Core.csproj ProjectName.Core/
# Add additional project files as needed

# Restore dependencies - SOLUTION LEVEL for multi-project dependencies
RUN dotnet restore *.sln

# Copy source code
COPY . .

# Build the solution - SOLUTION LEVEL to validate all projects
RUN dotnet build *.sln -c Release -o /app/build

# ========================================
# Stage 2: Publish
# ========================================
FROM build AS publish
RUN dotnet publish ProjectName.Api/ProjectName.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# ========================================
# Stage 3: Runtime
# ========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for health checks and clean up apt cache to minimize layer size
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

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
ENTRYPOINT ["dotnet", "ProjectName.Api.dll"]
```

**Key Best Practices:**
- ✅ **Solution-wide restore/build**: Handles multi-project dependencies correctly
- ✅ **Health checks**: Critical for production and orchestration
- ✅ **`/p:UseAppHost=false`**: .NET container best practice (smaller size, no native executable)
- ✅ **Proper permissions**: `chmod 755` on data directory prevents SQLite access issues
- ✅ **Apt cache cleanup**: `rm -rf /var/lib/apt/lists/*` minimizes layer size
- ✅ **Three-stage build**: Separates build/publish/runtime for optimal caching

### .dockerignore for .NET

```
**/.git
**/.gitignore
**/.vs
**/.vscode
**/bin
**/obj
**/.DS_Store
**/*.user
**/*.suo
.dockerignore
Dockerfile
.env
.env.*.local
```

---

## Python Backend Template

### Dockerfile Pattern (Flask/FastAPI)

```dockerfile
# Multi-stage build for Python API
FROM python:3.14-alpine AS build

WORKDIR /app

# Install build dependencies
RUN apk add --no-cache gcc musl-dev libffi-dev

# Copy requirements
COPY requirements.txt .

# Install Python dependencies
RUN pip install --no-cache-dir --user -r requirements.txt

# Runtime stage
FROM python:3.14-alpine

WORKDIR /app

# Copy dependencies from build stage
COPY --from=build /root/.local /root/.local

# Copy application
COPY . .

# Make sure scripts are in PATH
ENV PATH=/root/.local/bin:$PATH

EXPOSE 5000

# Health check
HEALTHCHECK --interval=30s --timeout=10s --retries=3 --start-period=40s \
  CMD python -c "import requests; requests.get('http://localhost:5000/health')" || exit 1

CMD ["python", "app.py"]
```

### .dockerignore for Python

```
**/.git
**/.gitignore
**/__pycache__
**/*.pyc
**/*.pyo
**/*.pyd
**/.Python
**/venv
**/env
**/.venv
**/.env
**/.DS_Store
**/dist
**/build
**/*.egg-info
.dockerignore
Dockerfile
docker-compose.yml
.pytest_cache
.coverage
```

---

## React Frontend Template

### Dockerfile Pattern (React/Vite with Nginx)

**EFFICIENCY OPTIMIZED:** ARG/ENV placement after source copy prevents invalidating npm ci cache when environment variables change.

```dockerfile
# Multi-stage build for React/Vite frontend
FROM node:20-alpine AS build

WORKDIR /app
COPY package*.json ./
RUN npm ci

COPY . .

# Accept build arguments for Vite environment variables
# Placed AFTER COPY . . to optimize layer caching
# Changing these values won't invalidate npm ci cache
ARG VITE_API_URL=http://localhost:5000
ARG VITE_APP_NAME=My App
ENV VITE_API_URL=${VITE_API_URL}
ENV VITE_APP_NAME=${VITE_APP_NAME}

RUN npm run build

# Production runtime with Nginx
FROM nginx:alpine

COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

**Key Efficiency Features:**
- ARG/ENV after source copy = better cache utilization (~30% faster rebuilds)
- Minimal runtime stage = smaller image size (~43MB vs ~47MB)
- No extra packages (wget) unless health checks required in compose file
- Node 20 LTS for stability

### nginx.conf for React SPA

```nginx
server {
    listen 80;
    server_name localhost;
    root /usr/share/nginx/html;
    index index.html;

    # Gzip compression
    gzip on;
    gzip_types text/plain text/css application/json application/javascript text/xml application/xml application/xml+rss text/javascript;

    # Main location block for SPA
    location / {
        try_files $uri $uri/ /index.html;
    }

    # Cache static assets
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2|ttf|eot)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }

    # Security headers
    add_header X-Frame-Options "SAMEORIGIN" always;
    add_header X-Content-Type-Options "nosniff" always;
    add_header X-XSS-Protection "1; mode=block" always;
}
```

### .dockerignore for React/Node

**CRITICAL:** DO NOT exclude tsconfig*.json or vite.config.* - these are required for the build!

```
**/.git
**/.gitignore
**/node_modules
**/npm-debug.log
**/dist
**/build
**/.DS_Store
**/.env
**/.env.*.local
**/coverage
**/*.tgz
.dockerignore
Dockerfile
docker-compose.yml
.vscode
.idea
README.md
```

**Common Mistake to Avoid:**
- ❌ **DO NOT** add `tsconfig*.json` to .dockerignore (build will fail)
- ❌ **DO NOT** add `vite.config.*` to .dockerignore (build will fail)
- ❌ **DO NOT** add `.eslintrc*` or `.prettierrc*` if used in build scripts

---

## docker-compose.yml Template

### Basic Backend Service

**IMPORTANT:** Requires .env file with all variables configured.

```yaml
services:
  backend:
    build:
      context: ./Backend
    container_name: ${APP_NAME}-backend
    ports:
      - "${BACKEND_PORT}:5000"
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
      - ASPNETCORE_URLS=http://+:5000
      - Database__Provider=${DB_PROVIDER}
      - Database__SqliteDatabasePath=${DB_PATH}
      - Database__PostgresHost=${DB_HOST}
      - Database__PostgresPort=${DB_PORT}
      - Database__PostgresDatabase=${DB_NAME}
      - Database__PostgresUsername=${DB_USER}
      - Database__PostgresPassword=${DB_PASSWORD}
      - Jwt__Key=${JWT_KEY}
      - Jwt__Issuer=${JWT_ISSUER}
      - Jwt__Audience=${JWT_AUDIENCE}
      - Jwt__ExpiryInMinutes=${JWT_EXPIRY_MINUTES}
    volumes:
      - backend-data:/app/data
    networks:
      - app-network
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:5000/health"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s
    restart: unless-stopped

networks:
  app-network:
    driver: bridge

volumes:
  backend-data:
    driver: local
```

### With PostgreSQL Database

**IMPORTANT:** Requires .env file with all variables configured.

```yaml
services:
  backend:
    build:
      context: ./Backend
    container_name: ${APP_NAME}-backend
    ports:
      - "${BACKEND_PORT}:5000"
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
      - DATABASE_URL=postgresql://${DB_USER}:${DB_PASSWORD}@db:${DB_PORT}/${DB_NAME}
      - Jwt__Key=${JWT_KEY}
      - Jwt__Issuer=${JWT_ISSUER}
      - Jwt__Audience=${JWT_AUDIENCE}
    depends_on:
      db:
        condition: service_healthy
    networks:
      - app-network
    restart: unless-stopped

  db:
    image: postgres:15-alpine
    container_name: ${APP_NAME}-db
    environment:
      - POSTGRES_USER=${DB_USER}
      - POSTGRES_PASSWORD=${DB_PASSWORD}
      - POSTGRES_DB=${DB_NAME}
    volumes:
      - postgres_data:/var/lib/postgresql/data
    networks:
      - app-network
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${DB_USER}"]
      interval: 10s
      timeout: 5s
      retries: 5
    restart: unless-stopped

networks:
  app-network:
    driver: bridge

volumes:
  postgres_data:
    driver: local
```

### Full-Stack (Frontend + Backend)

**IMPORTANT:** Requires .env file with all variables configured.
**NOTE:** PostgreSQL or other databases should be managed externally. SQLite data is persisted via bind mount.

```yaml
services:
  backend:
    build:
      context: ./Backend
    container_name: flowinglinks-backend
    ports:
      - "${BACKEND_PORT}:5000"
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
      - ASPNETCORE_URLS=http://+:5000
      - Database__Provider=${DB_PROVIDER}
      - Database__SqliteDatabasePath=${DB_PATH}
      - Database__PostgresHost=${DB_HOST}
      - Database__PostgresPort=${DB_PORT}
      - Database__PostgresDatabase=${DB_NAME}
      - Database__PostgresUsername=${DB_USER}
      - Database__PostgresPassword=${DB_PASSWORD}
      - Jwt__Key=${JWT_KEY}
      - Jwt__Issuer=${JWT_ISSUER}
      - Jwt__Audience=${JWT_AUDIENCE}
      - Jwt__ExpiryInMinutes=${JWT_EXPIRY_MINUTES}
    volumes:
      - backend-data:/app/data
    networks:
      - flowinglinks-network
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:5000/health"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s
    restart: unless-stopped

  frontend:
    build:
      context: ./Frontend
      args:
        - VITE_API_URL=${VITE_API_URL}
        - VITE_APP_NAME=${VITE_APP_NAME}
    container_name: flowinglinks-frontend
    ports:
      - "${FRONTEND_PORT}:80"
    depends_on:
      - backend
    networks:
      - flowinglinks-network
    restart: unless-stopped

networks:
  flowinglinks-network:
    driver: bridge

volumes:
  backend-data:
    driver: local
```

---

## Key Configuration Patterns

### Environment Variables

**CRITICAL:** Do NOT use defaults. Require variables from .env file:
```yaml
# CORRECT - requires .env file
- "${VARIABLE_NAME}"

# WRONG - do not use defaults
- "${VARIABLE_NAME:-default_value}"
```

All variables must be defined in .env file before running containers.

### .NET Configuration Property Name Verification

**CRITICAL FOR .NET PROJECTS:** Always verify configuration class property names before generating docker-compose.yml:

```bash
# 1. Find all Settings/Config classes
grep -r "class.*Settings|record.*Settings" Backend --include="*.cs"

# 2. Read each class to extract exact property names
# Example: JwtSettings.cs has "ExpiryInMinutes" property

# 3. Map to hierarchical environment variable format:
# Property: ExpiryInMinutes → Environment Variable: Jwt__ExpiryInMinutes
```

**Common Mistakes:**
- ❌ Assuming property name is `ExpiryMinutes` when it's actually `ExpiryInMinutes`
- ❌ Using `Database__Path` when code expects `Database__SqliteDatabasePath`
- ❌ Not verifying property names from source code

**Correct Approach:**
1. Read the Settings class file
2. Extract exact property names
3. Use the exact names in hierarchical format (Section__PropertyName)

### Health Checks

Always include health checks for production:
```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:PORT/health"]
  interval: 30s        # How often to check
  timeout: 10s         # Max time for check to complete
  retries: 3           # Failed checks before unhealthy
  start_period: 40s    # Grace period on startup
```

### Restart Policies

```yaml
restart: unless-stopped  # Restart on failure, but not if manually stopped
restart: always          # Always restart
restart: on-failure      # Only restart on failure
restart: "no"            # Never restart
```

### Volume Mounts

**Prefer named volumes for data persistence:**

```yaml
volumes:
  - named_volume:/container/path     # Named volume (preferred for data)
  - ./local/path:/container/path     # Bind mount (use for development)

# Define named volumes at root level
volumes:
  named_volume:
    driver: local
```

Named volumes are Docker-managed and better isolated. Use bind mounts only for development or when direct file access is needed.

---

## Common Port Assignments

- ASP.NET Core: 5000, 5001 (HTTPS)
- Flask: 5000
- FastAPI: 8000
- React/Vite (dev): 5173
- React/Nginx (prod): 80, 443
- PostgreSQL: 5432
- Redis: 6379
- MongoDB: 27017

---

## Checklist

Before finalizing Docker setup:

- [ ] **CRITICAL:** .env.example generated with ALL variables documented
- [ ] **CRITICAL:** NO default values in docker-compose.yml (no `:-default` syntax)
- [ ] User instructed to copy .env.example to .env and configure
- [ ] Multi-stage build for optimal image size
- [ ] .dockerignore file present for each service
- [ ] Health check endpoint implemented (backend)
- [ ] Volume mounts for persistent data
- [ ] Proper networking between services
- [ ] Restart policy configured
- [ ] Port mappings documented
- [ ] Security: secrets not in Dockerfile
- [ ] Nginx config for SPA routing (frontend)
- [ ] Build args for environment variables (frontend)
- [ ] Frontend depends_on backend with health check
- [ ] CORS configured for frontend-backend communication

## Setup Instructions to Provide to User

After generating all Docker files, inform the user:

```
Docker configuration complete! Follow these steps:

1. Copy the example environment file:
   cp .env.example .env

2. Edit .env and configure ALL variables:
   - Set secure passwords and keys
   - Configure database settings
   - Set application-specific values

3. Build and run containers:
   docker-compose up --build

Note: Containers will FAIL to start if .env is missing or incomplete.
```
