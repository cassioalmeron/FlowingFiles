import { useState, useCallback, useEffect, useRef } from 'react';
import { saveAs } from 'file-saver';
import { toast } from 'react-toastify';
import type { FileEntry } from '../types';
import { API_URL } from '../lib/api';
import { generateZip } from '../lib/zip';

const MONTHS = [
  'January', 'February', 'March', 'April', 'May', 'June',
  'July', 'August', 'September', 'October', 'November', 'December',
];

export function useDocumentManager() {
  const [files, setFiles] = useState<FileEntry[]>([]);
  const [loading, setLoading] = useState(true);
  const [classifying, setClassifying] = useState(false);
  const [currentIndex, setCurrentIndex] = useState<number | null>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    fetch(`${API_URL}/documentoption`)
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json() as Promise<Array<{ description: string; path: string; required: boolean }>>;
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
    setFiles((prev) => prev.map((entry, i) => (i === index ? { ...entry, file } : entry)));
  }, []);

  const clearFile = useCallback((index: number) => {
    setFiles((prev) => prev.map((entry, i) => (i === index ? { ...entry, file: null } : entry)));
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

      const labels: string[] = await res.json();
      const fileArray = Array.from(selectedFiles);

      setFiles((prev) => {
        const updated = [...prev];
        fileArray.forEach((file, i) => {
          const label = labels[i];
          if (label === 'Unknown') {
            toast.warning(`Could not classify: ${file.name}`);
            return;
          }
          const slotIndex = updated.findIndex(
            (e) => e.option.description.toLowerCase() === label.toLowerCase()
          );
          if (slotIndex !== -1)
            updated[slotIndex] = { ...updated[slotIndex], file };
          else
            toast.warning(`No slot found for: ${file.name} (${label})`);
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

  const currentFile = currentIndex !== null ? files[currentIndex] : null;

  return {
    files,
    loading,
    classifying,
    currentIndex,
    currentFile,
    selectedMonth,
    monthAbbrev,
    fileInputRef,
    setCurrentIndex,
    setSelectedMonth,
    selectFile,
    clearFile,
    exportZip,
    autoClassify,
  };
}
