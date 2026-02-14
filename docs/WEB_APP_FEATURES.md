# FlowingFiles Web App - Feature Description

## Overview

FlowingFiles Web is a browser-based document organizer for Brazilian accounting workflows. Users collect monthly accounting, fiscal, and banking files into a structured checklist, verify completeness at a glance, preview documents inline, and export everything as a single ZIP archive — all without leaving the browser.

---

## Core Features

### 1. Document Checklist

The left panel displays a scrollable list of all required and optional documents organized by category:

- **Accounting (Contabil)** — Payroll slips, profit distribution receipts, and payment vouchers for DARF INSS, E-Cont, New Office, CSLL, and IRPJ
- **Fiscal** — Issued service invoices (NFSe) and purchased service invoices (E-Cont, New Office), each in both XML and PDF formats
- **Banking** — Bank statements, OFX transaction files, and monthly reports

Each document entry shows:
- The document name in bold
- An "optional" badge for non-mandatory items
- The selected file name, or "No file selected" if empty
- An upload button to attach a file
- A trash button to remove an attached file (appears only when a file is attached)

### 2. Visual Status Indicators

Every document in the checklist has a colored left border that indicates its status at a glance:

- **Green** — A file has been attached
- **Red** — A required file is still missing
- **Orange** — An optional file has not been attached (no action required)

This allows users to quickly scan the list and identify which documents still need to be provided before exporting.

### 3. File Selection

Users attach files to document entries by clicking the upload button on any item. This opens the browser's native file picker dialog. After selecting a file:

- The file name appears in the document entry
- The status border changes to green
- The file is held in memory for preview and export

Users can replace a file at any time by clicking the upload button again, or remove it by clicking the trash button.

### 4. Document Preview

The right panel shows a live preview of the currently selected document. Clicking any item in the checklist displays its content:

- **PDF files** are rendered in an embedded viewer with scroll and zoom support
- **Images** (PNG, JPG, GIF, etc.) are displayed centered and scaled to fit
- **Text-based files** (XML, OFX, TXT, CSV, JSON) are shown as formatted text with a monospace font
- **Other file types** display the file name with a message that preview is not available

When no file is selected, the preview area shows a placeholder prompting the user to select a file.

### 5. Month Selector

A dropdown at the bottom of the screen lets users choose the reference month (January through December). It defaults to the current month. The selected month determines the name of the exported ZIP file (e.g., selecting March produces `Mar.zip`).

### 6. Export as ZIP

The "Export as ZIP" button generates a compressed archive containing all attached files, organized into the same folder structure used by the accounting workflow:

```
Mar.zip
├── Contabil/
│   ├── Folha de Pagamento/
│   │   ├── Contra Cheque.pdf
│   │   ├── PIX - Divisao de Lucros.pdf
│   │   └── PIX - Folha de Pagamento.pdf
│   └── Pagamentos/
│       ├── DARF_INSS - Boleto.pdf
│       ├── DARF_INSS - Comprovante.pdf
│       └── ...
├── Fiscal/
│   ├── Notas Fiscais Emitidas/
│   │   └── NFSE.xml / NFSE.pdf
│   └── Notas Fiscais Servicos Tomados/
│       └── ...
├── Extrato.pdf
├── Movimentacoes.ofx
└── Relatorio.pdf
```

The ZIP downloads automatically through the browser. The button is disabled when no files have been attached.

The export action is also accessible from the menu bar at the top of the screen.

### 7. Progress Tracking

A counter in the toolbar shows how many files have been attached out of the total (e.g., "5 / 22 files"), giving users a quick sense of how much work remains.

The document list header also displays the same count for at-a-glance reference.

---

## User Experience Flow

1. **Open the app** in any modern browser — no installation or account needed
2. **Work through the checklist** from top to bottom, clicking the upload button on each entry and selecting the corresponding file from your computer
3. **Check for missing items** by scanning for red borders (required) or orange borders (optional)
4. **Preview any file** by clicking its row in the checklist — the right panel shows the document content
5. **Select the month** from the dropdown at the bottom
6. **Click "Export as ZIP"** — a structured ZIP file downloads to your computer, ready to send to your accountant

---

## Visual Layout

The interface is divided into three areas:

- **Menu Bar** (top) — Displays the app name and provides quick access to the export action
- **Document List** (left panel, ~480px wide) — Scrollable checklist of all document entries with status indicators
- **Preview & Toolbar** (right panel) — File preview area on top with the month selector and export button at the bottom

The app uses a dark theme with a deep charcoal background, light gray text, and steel-blue accent colors for interactive elements.

---

## Business Value

- **Eliminates manual file organization** — No more creating folders by hand each month
- **Reduces missing documents** — Color-coded status makes gaps immediately visible
- **Works anywhere** — Runs in the browser with no installation, backend, or account required
- **Fast delivery** — One-click ZIP export produces a ready-to-send archive
- **Privacy-first** — All files stay in the browser; nothing is uploaded to a server
