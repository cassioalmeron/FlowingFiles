import { useState, useCallback, useEffect } from 'react';
import JSZip from 'jszip';
import { saveAs } from 'file-saver';
import { toast } from 'react-toastify';
import type { FileEntry } from '../types';
import { API_URL } from '../lib/api';

const MONTHS = [
  'January', 'February', 'March', 'April', 'May', 'June',
  'July', 'August', 'September', 'October', 'November', 'December',
];

export function useDocumentManager() {
  const [files, setFiles] = useState<FileEntry[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentIndex, setCurrentIndex] = useState<number | null>(null);

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
      const zip = new JSZip();

      for (const entry of filledEntries) {
        const ext = entry.file!.name.includes('.')
          ? '.' + entry.file!.name.split('.').pop()
          : '';
        const zipPath = `${entry.option.path}${ext}`;
        const buffer = await entry.file!.arrayBuffer();
        zip.file(zipPath, buffer);
      }

      const blob = await zip.generateAsync({ type: 'blob' });
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

  const currentFile = currentIndex !== null ? files[currentIndex] : null;

  return {
    files,
    loading,
    currentIndex,
    currentFile,
    selectedMonth,
    monthAbbrev,
    setCurrentIndex,
    setSelectedMonth,
    selectFile,
    clearFile,
    exportZip,
  };
}
