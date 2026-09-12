# FlowingFiles

An application for organizing and managing monthly accounting and fiscal documents for Brazilian businesses. It streamlines collecting the required files, auto-detecting/classifying them by content, exporting them in a structured ZIP, and emailing the result to the accountant.

Available as a **web app** (React + .NET API, PostgreSQL) and a legacy **Windows desktop app** (WPF).

## Features

- **Document Checklist** — configurable list of required and optional documents across categories (Accounting, Fiscal, Banking)
- **Auto-Classification** — uploaded PDFs/images are matched against a labelled sample corpus using text embeddings (Ollama) and cosine similarity, and placed into the correct slot automatically; XML invoices are classified by CNPJ
- **Training Samples** — a page to build and curate the labelled corpus the classifier compares against (upload, view, delete, with confirmation)
- **Batch Ingest** — backfill the corpus from several previously-exported monthly ZIPs at once
- **File Preview** — preview selected documents (PDF, images, XML, OFX)
- **Export / Import as ZIP** — download all collected files as a structured ZIP archive, or reload a previously exported archive back into the page
- **Email Registration & Sending** — register recipient addresses and send the collected files by email (Gmail)
- **Month Selector** — exports are named after the selected reference month (e.g., `Jan.zip`)
- **Dark Theme**

---

## Web App

A React frontend backed by an ASP.NET Core API. Uploaded files, samples and configuration are persisted through the API; ZIP export/import still happens entirely in the browser.

### Requirements

- [Node.js](https://nodejs.org/) 18+
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL with the [pgvector](https://github.com/pgvector/pgvector) extension (or leave `DATABASE_URL` unset to fall back to a local SQLite file)
- [Ollama](https://ollama.com/) running and reachable, with the `bge-m3` model pulled — used to embed document text for classification
- `tesseract` CLI on `PATH` — used for OCR on image uploads

### Environment Variables

Copy `.env.example` to `.env` at the repository root and fill in the values. Used by both the API and Docker Compose:

| Variable | Purpose |
|---|---|
| `DATABASE_URL` | PostgreSQL connection string. Empty → falls back to a local SQLite file |
| `DB_PATH` | Optional SQLite file path, only used when `DATABASE_URL` is empty |
| `GMAIL_USER` / `GMAIL_APP_PASSWORD` | Gmail account and app password used to send emails |
| `OLLAMA_BASE_URL` / `OLLAMA_EMBEDDING_MODEL` | Override the embedding service location/model (defaults live in `server/FlowingFiles.Api/appsettings.json`) |
| `OTEL_EXPORTER_OTLP_ENDPOINT` | Optional. When set, the API exports OpenTelemetry traces/metrics there; unset by default (no export) |
| `ASPNETCORE_ENVIRONMENT` | ASP.NET Core hosting environment |
| `APP_NAME`, `API_PORT`, `WEB_PORT` | Container names and published ports (Docker Compose) |
| `VITE_APP_NAME`, `VITE_API_URL` | Build arguments for the web image (Docker Compose) |

See `docs/documentation/environment-configuration.md` for full details.

### Getting Started with Docker

```bash
docker compose up -d --build
```

This builds and runs the `api` and `web` containers. Ollama and PostgreSQL are **not** included in
`docker-compose.yml` — they must already be running and reachable from the containers.

### Getting Started without Docker

```bash
# Backend (from server/FlowingFiles.Api) — applies pending EF Core migrations automatically on startup
dotnet run --project FlowingFiles.Api

# Frontend (from web/), in another terminal
npm install
npm run dev
```

Open `http://localhost:5173` in your browser.

### Available Scripts (`web/`)

| Script | Description |
|---|---|
| `npm run dev` | Start the Vite dev server |
| `npm run build` | Type-check (`tsc -b`) and build for production |
| `npm run lint` | Run ESLint |
| `npm run preview` | Preview a production build locally |

### Build for Production

```bash
cd web
npm run build
```

The output is in `web/dist/`, served by the `web` container (`nginx`) or any static host — it expects the API to be reachable at `VITE_API_URL`.

### Observability (optional)

The API is instrumented with OpenTelemetry (ASP.NET Core, HttpClient and EF Core tracing/metrics), but doesn't export anywhere until `OTEL_EXPORTER_OTLP_ENDPOINT` is set. To view traces locally:

```powershell
./scripts/otel-dashboard.ps1
```

This starts a local Aspire Dashboard container at `http://localhost:18888` and prints the OTLP endpoint to set. Run with `-Remove` to tear it down. Windows PowerShell only.

### Running Tests

```bash
cd server
dotnet test FlowingFiles.Tests/FlowingFiles.Tests.csproj
```

### Tech Stack

- **Frontend:** React 19, TypeScript, Vite, React Router, react-toastify, JSZip, file-saver
- **Backend:** .NET 8 / ASP.NET Core, Entity Framework Core 8
- **Database:** PostgreSQL + pgvector (production), SQLite (local fallback)
- **Classification:** Ollama (`bge-m3` embeddings), pgvector cosine similarity, Tesseract OCR
- **Email:** MailKit (Gmail SMTP)
- **Observability:** Serilog (file sink), OpenTelemetry (tracing/metrics, optional OTLP export)

### Project Structure

```
server/
├── FlowingFiles.Api/       # Controllers, Program.cs, appsettings, Dockerfile
├── FlowingFiles.Core/      # DbContext, Models, EF Configurations, Migrations, Services, Dtos
├── FlowingFiles.Console/   # Scratch console project for manual prototyping
└── FlowingFiles.Tests/     # Unit tests (MSTest)
web/src/
├── components/
│   ├── icons.tsx                      # SVG icon components
│   ├── layout/AppLayout/              # Shared page chrome
│   └── features/                      # Components shared by 2+ pages
├── pages/
│   ├── Upload/                        # Main working page — attach files, classify, export
│   ├── FilesConfiguration/            # CRUD for the document checklist
│   ├── EmailRegistration/             # Manage recipient addresses
│   ├── SamplesManagement/             # Curate the classifier's training corpus
│   └── BatchIngest/                   # Backfill the corpus from exported ZIPs
├── hooks/useDocumentManager.ts        # Core state: files, selection, ZIP export/import, sample ingestion
├── lib/                                # zip.ts (ZIP generation/parsing), api.ts (API_URL)
├── types/                              # Shared TypeScript types
└── App.tsx                            # Routes
```

See `docs/documentation/index.md` for per-feature technical docs and `docs/plans/` for the implementation plans behind each major feature.

---

## Desktop App (WPF) — Legacy

The original Windows desktop application with native file dialogs and folder export. Superseded by the web app; kept buildable but not actively developed.

### Requirements

- Windows 10 or later
- [.NET 6.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/6.0)
- [WebView2 Runtime](https://developer.microsoft.com/en-us/microsoft-edge/webview2/) (usually pre-installed on Windows 10+)

### Build

```bash
dotnet build FlowingFiles/FlowingFiles.csproj
```

To publish a self-contained executable:

```bash
dotnet publish FlowingFiles/FlowingFiles.csproj -c Release -r win-x64
```

### Usage

1. Launch the application.
2. For each document in the list, click **Open File** to select the corresponding file from your system.
3. Verify all required items are green. Red items indicate missing required documents.
4. Select the reference **month** from the dropdown at the bottom.
5. Click **Export** to generate an organized folder at `C:\Temp\Flowing\{Month}`, or **Export as ZIP** to create a compressed archive.

Right-click a file entry and select **Clean** to clear a previously selected file.

### Tech Stack

- **C# / WPF** with MVVM architecture
- **.NET 6.0** (Windows)
- **Microsoft.Web.WebView2** for document preview
- **System.Windows.Interactivity** for XAML behaviors

### Project Structure

```
FlowingFiles/
├── MVVM/
│   ├── MainViewModel.cs        # Main application logic and commands
│   ├── FileViewModel.cs        # Individual file model with status tracking
│   ├── OptionViewModel.cs      # Document category definitions
│   ├── ViewModelBase.cs        # INotifyPropertyChanged base class
│   └── RelayCommand.cs         # ICommand implementation
├── Behaviors/
│   ├── OpenFileBehavior.cs     # File dialog trigger
│   └── WebViewBehavior.cs      # WebView2 file preview binding
├── Converters/
│   ├── BorderColorConverter.cs # Status to border color mapping
│   ├── NotNullToVisibilityConverter.cs
│   └── StringToUriConverter.cs
├── Themes/
│   ├── ColourDictionaries/     # Theme color definitions
│   ├── Controls.xaml           # Global control styles
│   ├── ControlColours.xaml     # Control-specific brushes
│   ├── ThemeType.cs            # Theme enumeration
│   └── ThemesController.cs    # Theme switching logic
├── MainWindow.xaml             # Main UI layout
├── App.xaml                    # Application entry point
└── FileStatusEnum.cs           # File status enumeration
```

---

## License

This project is for personal/internal use.
