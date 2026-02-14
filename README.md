# FlowingFiles

An application for organizing and managing monthly accounting and fiscal documents for Brazilian businesses. It streamlines the process of collecting required files and exporting them in a structured folder hierarchy for accountants and tax filing.

Available as a **Windows desktop app** (WPF) and a **web app** (React).

## Features

- **Document Checklist** — Predefined list of required and optional documents across categories:
  - **Accounting (Contabil):** Payroll, payment receipts (DARF INSS, CSLL, IRPJ, E-Cont, New Office)
  - **Fiscal:** NFSe (issued service invoices), purchased service notes (XML and PDF)
  - **Banking:** Account statements and OFX transaction files
- **Visual Status Indicators** — Color-coded borders show document status at a glance:
  - Green = file attached
  - Red = required file missing
  - Orange = optional file missing
- **File Preview** — Preview selected documents (PDF, images, XML, OFX)
- **Export as ZIP** — Download all collected files as a structured ZIP archive
- **Month Selector** — Choose the reference month; exports are named accordingly (e.g., `Jan`, `Feb`)
- **Dark Theme** — DeepDark theme with steel-blue accents

---

## Web App (React)

A browser-based version that runs entirely on the client — no backend required. Files are held in memory and exported as a ZIP download.

### Requirements

- [Node.js](https://nodejs.org/) 18+

### Getting Started

```bash
cd web
npm install
npm run dev
```

Open `http://localhost:5173` in your browser.

### Build for Production

```bash
cd web
npm run build
```

The output is in `web/dist/` and can be served from any static hosting.

### Tech Stack

- **React 18** with **TypeScript**
- **Vite** as build tool
- **CSS Modules** with CSS custom properties (DeepDark theme)
- **JSZip** for client-side ZIP generation
- **file-saver** for triggering downloads

### Project Structure

```
web/src/
├── components/
│   ├── icons.tsx                       # SVG icon components
│   └── features/
│       ├── MenuBar/                    # Top bar with brand and export action
│       ├── FileList/                   # Scrollable document list
│       ├── FileListItem/              # Individual document row with status
│       ├── FilePreview/               # Right panel (PDF, image, text preview)
│       └── Toolbar/                   # Bottom bar with month selector and export
├── hooks/
│   └── useDocumentManager.ts          # Core state: files, selection, ZIP export
├── data/
│   └── documentOptions.ts            # 22 document definitions (from WPF app)
├── types/
│   └── index.ts                       # FileStatus, DocumentOption, FileEntry
├── styles/
│   ├── variables.css                  # DeepDark theme CSS custom properties
│   └── globals.css                    # Reset and base styles
├── App.tsx                            # Main two-column layout
└── main.tsx                           # Entry point
```

---

## Desktop App (WPF)

The original Windows desktop application with native file dialogs and folder export.

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
