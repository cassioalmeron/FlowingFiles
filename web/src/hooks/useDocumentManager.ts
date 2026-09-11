import { useState, useCallback, useEffect, useRef } from 'react';
import { saveAs } from 'file-saver';
import { toast } from 'react-toastify';
import type { FileClassificationResult, FileEntry } from '../types';
import { API_URL } from '../lib/api';
import { generateZip, matchEntriesToOptions, parseZip } from '../lib/zip';

const MONTHS = [
  'January', 'February', 'March', 'April', 'May', 'June',
  'July', 'August', 'September', 'October', 'November', 'December',
];

export function useDocumentManager() {
  const [files, setFiles] = useState<FileEntry[]>([]);
  const [loading, setLoading] = useState(true);
  const [classifying, setClassifying] = useState(false);
  const [importing, setImporting] = useState(false);
  const [ingesting, setIngesting] = useState(false);
  const [currentIndex, setCurrentIndex] = useState<number | null>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);
  const zipInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    fetch(`${API_URL}/documentoption`)
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json() as Promise<Array<{ id: number; description: string; path: string; required: boolean }>>;
      })
      .then((data) => {
        setFiles(data.map((option) => ({ option, file: null })));
      })
      .catch(() => {
        toast.error('Failed to load document options. Please refresh the page.');
        setFiles([]);
      })
      .finally(() => setLoading(false));
  }, []);
  const [selectedMonth, setSelectedMonth] = useState<number>(new Date().getMonth() + 1);

  const selectFile = useCallback((index: number, file: File) => {
    // A manually chosen file invalidates any classification info from a previous auto-classify run.
    setFiles((prev) => prev.map((entry, i) => (i === index ? { ...entry, file, classification: undefined } : entry)));
  }, []);

  const clearFile = useCallback((index: number) => {
    setFiles((prev) => prev.map((entry, i) => (i === index ? { ...entry, file: null, classification: undefined } : entry)));
  }, []);

  const monthAbbrev = MONTHS[selectedMonth - 1].substring(0, 3);

  const exportZip = useCallback(async () => {
    const filledEntries = files.filter((e) => e.file !== null);

    if (filledEntries.length === 0) {
      toast.warning('No files selected. Please attach at least one file before exporting.');
      return;
    }

    const toastId = toast.loading('Generating ZIP...');

    try {
      const blob = await generateZip(files, monthAbbrev);
      saveAs(blob, `${monthAbbrev}.zip`);

      toast.update(toastId, {
        render: `${monthAbbrev}.zip exported successfully!`,
        type: 'success',
        isLoading: false,
        autoClose: 3000,
      });
    } catch {
      toast.update(toastId, {
        render: 'Failed to generate ZIP. Please try again.',
        type: 'error',
        isLoading: false,
        autoClose: 3000,
      });
    }
  }, [files, monthAbbrev]);

  const autoClassify = useCallback(async (selectedFiles: FileList) => {
    const toastId = toast.loading('Classifying files...');
    setClassifying(true);

    try {
      const formData = new FormData();
      Array.from(selectedFiles).forEach((file) => formData.append('files', file));

      const res = await fetch(`${API_URL}/File/classify`, { method: 'POST', body: formData });
      if (!res.ok) throw new Error(`HTTP ${res.status}`);

      const classifications: FileClassificationResult[] = await res.json();
      const fileArray = Array.from(selectedFiles);

      setFiles((prev) => {
        const updated = [...prev];
        fileArray.forEach((file, i) => {
          const classification = classifications[i];
          if (classification.label === 'Unknown') {
            toast.warning(`Could not classify: ${file.name}`);
            return;
          }
          const slotIndex = updated.findIndex(
            (e) => e.option.description.toLowerCase() === classification.label.toLowerCase()
          );
          if (slotIndex !== -1)
            updated[slotIndex] = { ...updated[slotIndex], file, classification };
          else
            toast.warning(`No slot found for: ${file.name} (${classification.label})`);
        });
        return updated;
      });

      toast.update(toastId, { render: 'Files classified!', type: 'success', isLoading: false, autoClose: 3000 });
    } catch {
      toast.update(toastId, { render: 'Classification failed. Please try again.', type: 'error', isLoading: false, autoClose: 3000 });
    } finally {
      setClassifying(false);
    }
  }, []);

  const importZip = useCallback(async (archive: File) => {
    const toastId = toast.loading('Loading ZIP...');
    setImporting(true);

    try {
      const { entries, monthAbbrev } = await parseZip(archive);
      const { matchesByOptionId, unmatched } = matchEntriesToOptions(entries, files.map((f) => f.option));

      const updated = files.map((entry) => ({
        ...entry,
        file: matchesByOptionId.get(entry.option.id)?.file ?? null,
        classification: undefined,
      }));

      setFiles(updated);
      setCurrentIndex(null);

      const monthIndex = monthAbbrev
        ? MONTHS.findIndex((name) => name.substring(0, 3) === monthAbbrev)
        : -1;
      if (monthIndex !== -1) setSelectedMonth(monthIndex + 1);

      toast.update(toastId, {
        render: `${matchesByOptionId.size} of ${entries.length} files loaded from the archive.`,
        type: matchesByOptionId.size === 0 ? 'warning' : 'success',
        isLoading: false,
        autoClose: 3000,
      });

      unmatched.forEach((e) => toast.warning(`No slot found for: ${e.path}`));
    } catch {
      toast.update(toastId, {
        render: 'Failed to read the ZIP. Please select an archive exported by this application.',
        type: 'error',
        isLoading: false,
        autoClose: 3000,
      });
    } finally {
      setImporting(false);
    }
  }, [files]);

  // Bulk route into DocumentSample (Design Decision 7, plan 003): every filled slot becomes one
  // POST /documentsample call. .xml entries are skipped — they're classified by CNPJ and never
  // reach the embedding path, so there is nothing useful to ingest for them.
  const ingestSamples = useCallback(async () => {
    const candidates = files.filter(
      (e) => e.file !== null && !e.file.name.toLowerCase().endsWith('.xml')
    );

    if (candidates.length === 0) {
      toast.warning('No files to ingest as samples.');
      return;
    }

    const toastId = toast.loading('Ingesting samples...');
    setIngesting(true);
    let succeeded = 0;

    try {
      for (const entry of candidates) {
        try {
          const formData = new FormData();
          formData.append('documentOptionId', String(entry.option.id));
          formData.append('files', entry.file as File);

          const res = await fetch(`${API_URL}/documentsample`, { method: 'POST', body: formData });
          if (!res.ok) throw new Error(`HTTP ${res.status}`);
          succeeded++;
        } catch {
          toast.warning(`Failed to ingest: ${entry.file!.name}`);
        }
      }

      toast.update(toastId, {
        render: `${succeeded} of ${candidates.length} samples ingested.`,
        type: succeeded === candidates.length ? 'success' : 'warning',
        isLoading: false,
        autoClose: 3000,
      });
    } finally {
      setIngesting(false);
    }
  }, [files]);

  const currentFile = currentIndex !== null ? files[currentIndex] : null;

  return {
    files,
    loading,
    classifying,
    importing,
    ingesting,
    currentIndex,
    currentFile,
    selectedMonth,
    monthAbbrev,
    fileInputRef,
    zipInputRef,
    setCurrentIndex,
    setSelectedMonth,
    selectFile,
    clearFile,
    exportZip,
    autoClassify,
    importZip,
    ingestSamples,
  };
}
