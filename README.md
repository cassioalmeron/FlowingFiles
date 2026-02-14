# FlowingFiles

A Windows desktop application for organizing and managing monthly accounting and fiscal documents for Brazilian businesses. It streamlines the process of collecting required files and exporting them in a structured folder hierarchy for accountants and tax filing.

## Features

- **Document Checklist** — Predefined list of required and optional documents across categories:
  - **Accounting (Contabil):** Payroll, payment receipts (DARF INSS, CSLL, IRPJ, E-Cont, New Office)
  - **Fiscal:** NFSe (issued service invoices), purchased service notes (XML and PDF)
  - **Banking:** Account statements and OFX transaction files
- **Visual Status Indicators** — Color-coded borders show document status at a glance:
  - Green = file attached
  - Red = required file missing
  - Orange = optional file missing
- **File Preview** — Built-in WebView2 panel to preview selected documents
- **Export Options** — Export collected files as an organized folder structure or a ZIP archive
- **Month Selector** — Choose the reference month; exports are named accordingly (e.g., `Jan`, `Feb`)
- **Dark Theme** — Ships with multiple theme options (DeepDark, SoftDark, RedBlack, Grey, DarkGrey, Light)

## Requirements

- Windows 10 or later
- [.NET 6.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/6.0)
- [WebView2 Runtime](https://developer.microsoft.com/en-us/microsoft-edge/webview2/) (usually pre-installed on Windows 10+)

## Build

```bash
dotnet build FlowingFiles/FlowingFiles.csproj
```

To publish a self-contained executable:

```bash
dotnet publish FlowingFiles/FlowingFiles.csproj -c Release -r win-x64
```

## Usage

1. Launch the application.
2. For each document in the list, click **Open File** to select the corresponding file from your system.
3. Verify all required items are green. Red items indicate missing required documents.
4. Select the reference **month** from the dropdown at the bottom.
5. Click **Export** to generate an organized folder at `C:\Temp\Flowing\{Month}`, or **Export as ZIP** to create a compressed archive.

Right-click a file entry and select **Clean** to clear a previously selected file.

## Project Structure

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

## Tech Stack

- **C# / WPF** with MVVM architecture
- **.NET 6.0** (Windows)
- **Microsoft.Web.WebView2** for document preview
- **System.Windows.Interactivity** for XAML behaviors

## License

This project is for personal/internal use.
