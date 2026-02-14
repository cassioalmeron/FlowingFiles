---
name: docker-setup
description: Analyze projects and generate Docker configurations for .NET, Python backend applications and React frontend applications. Use when setting up containerization, creating Dockerfiles, docker-compose files, or optimizing Docker setups for full-stack applications.
allowed-tools: Read, Grep, Glob, Bash, Write, Edit
---

# Docker Setup Skill

## Purpose

Generate production-ready Docker configurations for full-stack projects:
- Multi-stage Dockerfiles optimized for .NET, Python, and React
- docker-compose.yml with proper networking and volumes
- .dockerignore files for efficient builds
- Environment variable configuration
- Health checks and restart policies
- Nginx configuration for React frontends

## When to Use

Invoke this skill when you need to:
- "Set up Docker for this project"
- "Generate a Dockerfile and docker-compose"
- "Create Docker configuration for my backend"
- "Add containerization to this application"
- "Dockerize my React frontend"
- "Set up full-stack Docker deployment"

## Analysis Steps

1. **VERIFY .env FILE EXISTS (CRITICAL FIRST STEP)**
   - Check if .env file exists in project root using glob **/.env
   - If .env does NOT exist, STOP and inform user they must create it from .env.example
   - DO NOT proceed with container setup until .env exists
   - Verify .env is in .gitignore

2. **Detect Project Type**
   - Look for .csproj/.sln files (.NET)
   - Look for requirements.txt/pyproject.toml (Python)
   - Look for package.json with React/Vite (React frontend)
   - Check project structure and dependencies
   - **Python projects:** Check for `selenium` in dependencies → triggers Chrome/Selenium setup

3. **Verify Configuration Class Property Names (.NET CRITICAL)**
   - Search for configuration classes (JwtSettings, DatabaseConfig, etc.)
   - Read each configuration class to extract exact property names
   - Map property names to environment variable format (e.g., `ExpiryInMinutes` → `Jwt__ExpiryInMinutes`)
   - DO NOT assume property names - ALWAYS verify from source code

4. **Identify Requirements**
   - Database needs (SQLite for containerized, PostgreSQL assumed external)
   - Port configuration
   - Environment variables (use hierarchical format for .NET)
   - Data persistence requirements (named volumes preferred for SQLite data)
   - **Selenium/Chrome needs:** If selenium detected, read code to verify browser binary path requirements

5. **Generate Configuration**
   - Create .env.example with all required variables
   - Create multi-stage Dockerfile
   - Set up docker-compose.yml with services (NO default values)
   - Add .dockerignore for build optimization
   - Configure health checks
   - Document that .env file is MANDATORY before running containers

6. **USE ARG FOR CONFIGURABLE VALUES (CRITICAL)**
   - **MANDATORY:** All configurable values (ports, URLs, etc.) MUST use ARG in Dockerfiles
   - Pass ARG values from docker-compose.yml using build args that reference .env variables
   - This eliminates hardcoded values and ensures single source of truth (.env file)
   - **Pattern:**
     - Dockerfile: `ARG API_PORT=5002` → `EXPOSE ${API_PORT}` → health check uses `${API_PORT}`
     - docker-compose.yml: `args: - API_PORT=${API_PORT}` (references .env)
   - **Benefits:** Port changes only require updating .env, validation becomes simpler

7. **VALIDATE CONFIGURATION CONSISTENCY (SIMPLIFIED)**
   - When using ARG properly, validation is simpler since .env is single source of truth
   - **VERIFY:**
     - All Dockerfiles use ARG for ports and configurable values (not hardcoded)
     - docker-compose.yml passes all ARGs from .env variables
     - .env file contains all required variables
     - Frontend VITE_API_URL uses correct host port mapping
   - **IF ISSUES FOUND:**
     - Report missing ARG declarations or hardcoded values
     - Ensure docker-compose.yml build args reference .env variables

## .NET Project Pattern

Multi-stage build (PRODUCTION OPTIMIZED):
- Stage 1: SDK image for restore/build (SOLUTION-WIDE for multi-project dependencies)
- Stage 2: Publish stage with /p:UseAppHost=false flag
- Stage 3: Runtime image for production
- Install curl and clean apt cache (for health checks, minimize size)
- Create data directories with chmod 755 (SQLite permissions)
- **MANDATORY:** Use ARG for API_PORT with default value (e.g., `ARG API_PORT=5000`)
- Expose port using ARG: `EXPOSE ${API_PORT}`
- Health check using ARG: `CMD curl -f http://localhost:${API_PORT}/health || exit 1`
- Three-stage optimization for better caching
- Pass API_PORT as build arg in docker-compose.yml from .env

## Python Project Pattern

Single-stage build (PRODUCTION READY):
- Python 3.14 slim image (latest stable as of October 2025, or 3.12+ for compatibility)
- Install curl first for health checks, clean apt cache
- **CRITICAL:** Copy ALL code BEFORE pip install when using pyproject.toml with packages
  - If pyproject.toml defines `[tool.setuptools] packages = [...]`, package directories must exist
  - Correct order: COPY . . → RUN pip install --no-cache-dir .
  - DO NOT copy pyproject.toml alone first - will fail with "package directory does not exist"
- Create logs/data directories with appropriate permissions
- **MANDATORY:** Use ARG for API_PORT with default value (e.g., `ARG API_PORT=5002`)
- Expose port using ARG: `EXPOSE ${API_PORT}`
- Health check using ARG: `CMD curl -f http://localhost:${API_PORT}/health || exit 1`
- PYTHONUNBUFFERED=1 environment variable (hardcoded, not from .env)
- Pass API_PORT as build arg in docker-compose.yml from .env

### Python + Selenium/Chrome Pattern

**CRITICAL - When Selenium is Detected in Dependencies:**

1. **Detection Step:**
   - Check if `selenium` exists in requirements.txt or pyproject.toml dependencies
   - If found, read application code to understand Chrome configuration

2. **Chrome Browser Installation:**
   ```dockerfile
   # Install Chrome browser for Selenium (add after system dependencies)
   RUN apt-get update && apt-get install -y \
       curl \
       wget \
       gnupg \
       unzip \
       && wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | gpg --dearmor -o /usr/share/keyrings/google-chrome.gpg \
       && echo "deb [arch=amd64 signed-by=/usr/share/keyrings/google-chrome.gpg] http://dl.google.com/linux/chrome/deb/ stable main" > /etc/apt/sources.list.d/google-chrome.list \
       && apt-get update \
       && apt-get install -y google-chrome-stable \
       && rm -rf /var/lib/apt/lists/*

   # Note: ChromeDriver is automatically managed by Selenium Manager (Selenium 4.6+)
   # It will download the correct version matching the installed Chrome browser
   ```

3. **ChromeDriver Management - CRITICAL:**
   - **DO NOT manually install ChromeDriver**
   - Selenium 4.6+ includes Selenium Manager that automatically downloads the matching ChromeDriver version
   - Manual installation causes version mismatch errors (e.g., Chrome 143 vs ChromeDriver 131)
   - On first run, Selenium Manager downloads and caches the correct ChromeDriver version

4. **Environment Variable Naming - CRITICAL:**
   - **ALWAYS read the application code** to understand what the environment variable points to
   - Common naming mistake: `CHROME_DRIVER_PATH` or similar can point to EITHER:
     - Chrome **browser** binary path: `/usr/bin/google-chrome-stable` (for `binary_location` in ChromeOptions)
     - ChromeDriver executable path: `/usr/local/bin/chromedriver` (usually not needed with Selenium Manager)
   - Search for `ChromeOptions` or `webdriver.Chrome` in code to verify:
     - If `options.binary_location = os.getenv('CHROME_DRIVER_PATH')` → points to **browser** binary
     - If used in `webdriver.Chrome(executable_path=...)` → points to **driver** (deprecated pattern)
   - Set `.env.example` based on what the code expects:
     - Browser path: `CHROME_DRIVER_PATH=/usr/bin/google-chrome-stable`
     - Include comment: `# Chrome browser binary path (NOT ChromeDriver - this is the actual browser executable)`

5. **Headless Mode Configuration:**
   - Chrome arguments (`--headless`, `--no-sandbox`, `--disable-dev-shm-usage`) should be in application code
   - DO NOT add these to Dockerfile - they're runtime options, not build-time configuration

6. **Validation Checklist:**
   - [ ] Chrome browser installed from official Google repository
   - [ ] NO manual ChromeDriver installation in Dockerfile
   - [ ] Environment variable points to correct binary (browser vs driver)
   - [ ] .env.example has clear comment explaining what the path is for
   - [ ] selenium package version is 4.6+ (check requirements/pyproject.toml)

## React Project Pattern

Multi-stage build (EFFICIENCY OPTIMIZED):
- Stage 1: Node 20 LTS Alpine for npm install and build
- Stage 2: Nginx Alpine for serving static files (minimal, ~43MB)
- **CRITICAL:** Place ARG/ENV declarations AFTER `COPY . .` to optimize caching
  - Prevents invalidating npm ci cache when environment variables change
  - Improves rebuild time by ~30%
- Build args for ALL environment variables from .env (VITE_API_URL, VITE_APP_NAME, etc.) with defaults
- **MANDATORY:** Use ARG for backend API_PORT in nginx proxy configuration
  - Pattern: `ARG API_PORT=5002` in runtime stage
  - Nginx proxy_pass: `proxy_pass http://backend:${API_PORT}/;`
  - Pass API_PORT as build arg in docker-compose.yml from .env
- Nginx configuration for SPA routing
- Expose port 80
- No extra packages in runtime unless health checks required in compose

## docker-compose.yml Pattern

**IMPORTANT:** Do NOT generate PostgreSQL or other database containers. Assume databases are managed externally or via SQLite.

Include:
- Service definitions with build context
- **MANDATORY:** Build args for ALL configurable values (ports, URLs, etc.):
  - Backend: `args: - API_PORT=${API_PORT}` (from .env)
  - Frontend: `args: - VITE_API_URL=${VITE_API_URL}` and `- API_PORT=${API_PORT}` (from .env)
  - Use list format: `- ARG_NAME=${ENV_VAR}`
- Port mappings using .env variables (NO defaults)
- Environment variables from .env file (MANDATORY) using hierarchical format:
  - .NET: `Database__Provider`, `Database__SqliteDatabasePath`, `Jwt__Key`, etc.
  - Use list format: `- VARIABLE=value` not key-value pairs
- Named volumes for data persistence (preferred): `backend-data:/app/data`
- Volume definitions at root level with driver: local
- Network configuration
- Health checks (interval, timeout, retries, start_period)
- Restart policies (unless-stopped)
- Simple depends_on (no health check conditions)

## .env File Requirement

**CRITICAL:** All Docker configurations MUST require a .env file:
- Generate .env.example with all variables documented
- Use ${VARIABLE_NAME} syntax WITHOUT defaults in docker-compose.yml
- Containers will fail if .env is missing or variables are not set
- User must copy .env.example to .env and configure before running

## Best Practices

1. **CRITICAL (ALL):** ALWAYS use ARG for configurable values in Dockerfiles
   - Ports MUST use ARG: `ARG API_PORT=5002` → `EXPOSE ${API_PORT}`
   - Health checks MUST use ARG: `curl -f http://localhost:${API_PORT}/health`
   - Frontend nginx MUST use ARG: `proxy_pass http://backend:${API_PORT}/;`
   - NEVER hardcode ports or URLs - use ARG with defaults
2. **CRITICAL (ALL):** ALWAYS pass ARGs from docker-compose.yml using .env variables
   - Backend build args: `- API_PORT=${API_PORT}`
   - Frontend build args: `- VITE_API_URL=${VITE_API_URL}` and `- API_PORT=${API_PORT}`
   - This makes .env the single source of truth for all configuration
3. **CRITICAL (.NET):** ALWAYS verify configuration class property names before generating docker-compose.yml
   - Search for Settings/Config classes using grep: `grep -r "class.*Settings\|record.*Settings" --include="*.cs"`
   - Read each class to extract exact property names
   - Map to hierarchical format: PropertyName → Section__PropertyName
4. ALWAYS generate .env.example with all required variables
5. NEVER use default values in docker-compose.yml (${VAR:-default})
6. **CRITICAL:** Place ARG/ENV AFTER source copy in React Dockerfiles for optimal caching
7. **CRITICAL (.NET):** Use solution-wide restore/build for multi-project dependencies
8. **CRITICAL (.NET):** Include /p:UseAppHost=false in publish for container best practices
9. **CRITICAL (.NET):** Set chmod 755 on data directories to prevent SQLite access issues
10. **CRITICAL (React):** DO NOT exclude tsconfig*.json or vite.config.* in .dockerignore (build will fail)
11. PREFER named volumes over bind mounts for data persistence
12. Use Alpine images when possible for smaller size
13. Leverage build caching (COPY package files before source, ARG/ENV after source)
14. Use Node 20 LTS for React builds (not node:24 which doesn't exist)
15. Add comprehensive .dockerignore files (follow templates exactly)
16. Include health checks with curl and clean apt cache (rm -rf /var/lib/apt/lists/*)
17. Define named volumes at root level with driver: local
18. Document all environment variables with comments in .env.example
19. Include ARG defaults in Dockerfiles for flexibility (e.g., ARG VITE_API_URL=http://localhost:5000)
20. Use three-stage builds for .NET (build/publish/runtime) for optimal caching
21. **CRITICAL (Python/Selenium):** NEVER manually install ChromeDriver
   - Selenium Manager (4.6+) handles ChromeDriver automatically
   - Prevents version mismatch between Chrome and ChromeDriver
   - Manual installation causes "This version of ChromeDriver only supports Chrome version X" errors
22. **CRITICAL (Python/Selenium):** ALWAYS read code to verify environment variable usage
   - Check if variable points to browser binary (`binary_location`) or driver executable
   - Common mistake: CHROME_DRIVER_PATH pointing to driver instead of browser
   - Search for `options.binary_location` or `ChromeOptions` in code before setting .env values
23. **CRITICAL (Python/Selenium):** Install only Chrome browser in Dockerfile
   - Use official Google Chrome repository for google-chrome-stable
   - Let Selenium Manager download matching ChromeDriver at runtime
   - Include comment in Dockerfile: "ChromeDriver is automatically managed by Selenium Manager"

## Example Workflow

```bash
# STEP 1: VERIFY .env FILE EXISTS (MANDATORY)
glob **/.env

# If .env NOT found, STOP and inform user:
# "ERROR: .env file not found. Please copy .env.example to .env and configure all variables before setting up containers."
# DO NOT PROCEED until .env exists

# STEP 2: Analyze project structure
glob **/*.{csproj,sln,py,txt,toml,json}

# STEP 2a: DETECT SELENIUM DEPENDENCY (PYTHON PROJECTS)
# For Python projects, check if selenium is in dependencies
read Backend/requirements.txt or Backend/pyproject.toml
# If selenium found, proceed to Step 2b

# STEP 2b: ANALYZE SELENIUM CODE CONFIGURATION (IF SELENIUM DETECTED)
# Search for Selenium usage in code
grep -r "webdriver" Backend --include="*.py"
grep -r "ChromeOptions" Backend --include="*.py"
# Read files that use Selenium to understand configuration
read Backend/crowler.py or Backend/scraper.py or Backend/*selenium*.py
# Verify what environment variables are used:
# - Look for options.binary_location = os.getenv('...') → browser path needed
# - Look for webdriver.Chrome(executable_path=...) → driver path (deprecated)
# Set .env.example accordingly with clear comments

# STEP 3: VERIFY CONFIGURATION CLASS PROPERTY NAMES (.NET CRITICAL)
grep -r "class.*Settings|record.*Settings" Backend --include="*.cs"
read Backend/.../JwtSettings.cs           # Extract exact property names
read Backend/.../DatabaseConfig.cs        # Extract exact property names
# Map property names: ExpiryInMinutes → Jwt__ExpiryInMinutes

# STEP 4: Read configuration files
read Backend/FlowingLinks.Api/FlowingLinks.Api.csproj
read requirements.txt
read Frontend/package.json

# STEP 5: Generate configurations (in order)
write .env.example                    # FIRST - with all variables documented
write Backend/Dockerfile
write Frontend/Dockerfile
write Frontend/nginx.conf
write docker-compose.yml              # Uses variables from .env (no defaults)
write Backend/.dockerignore
write Frontend/.dockerignore

# STEP 6: VALIDATE ARG USAGE (MANDATORY BEFORE docker-compose up)
read Backend/Dockerfile               # Verify ARG API_PORT is declared and used
read Frontend/Dockerfile              # Verify ARG API_PORT is declared for nginx
read docker-compose.yml               # Verify build args pass from .env
read .env                             # Verify all required variables exist

# Check ARG usage:
# - Backend Dockerfile has ARG API_PORT declaration
# - Backend Dockerfile uses ${API_PORT} in EXPOSE and health check
# - Frontend Dockerfile has ARG API_PORT in runtime stage
# - Frontend nginx config uses ${API_PORT} in proxy_pass
# - docker-compose.yml passes API_PORT as build arg: - API_PORT=${API_PORT}
# - docker-compose.yml passes VITE_API_URL as build arg for frontend

# STEP 6a: VALIDATE SELENIUM SETUP (IF SELENIUM DETECTED)
read Backend/Dockerfile
# Verify Chrome/Selenium setup:
# - [ ] google-chrome-stable is installed from official Google repository
# - [ ] NO manual ChromeDriver installation commands (no wget chromedriver, no unzip)
# - [ ] Comment exists: "ChromeDriver is automatically managed by Selenium Manager"
# - [ ] curl, wget, gnupg, unzip are installed (for Chrome installation)
# - [ ] apt cache cleaned (rm -rf /var/lib/apt/lists/*)

read .env.example
# Verify environment variable comments:
# - [ ] CHROME_DRIVER_PATH or similar has clear comment explaining browser vs driver
# - [ ] Path points to /usr/bin/google-chrome-stable (browser) if using binary_location
# - [ ] Comment states: "Chrome browser binary path (NOT ChromeDriver...)"

# IF ISSUES FOUND:
# REPORT: Missing ARG declarations or hardcoded values
# ENSURE: All build args are passed from .env in docker-compose.yml
# ENSURE: ChromeDriver is NOT manually installed (let Selenium Manager handle it)

# STEP 7: Verify .env is configured
echo "IMPORTANT: Ensure .env is properly configured with all required variables before running docker-compose up"
```

See DOCKER-REFERENCE.md for detailed templates and patterns.
