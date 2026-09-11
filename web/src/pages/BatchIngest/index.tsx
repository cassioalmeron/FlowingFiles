import React, { useCallback, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { ArrowLeftIcon, ImportIcon } from '../../components/icons';
import { API_URL } from '../../lib/api';
import { matchEntriesToOptions, parseZip } from '../../lib/zip';
import type { DocumentOption } from '../../types';
import './styles.css';

interface ImportZipResponse {
  requested: number;
  ingested: number;
  failed: { path: string; reason: string }[];
}

interface ArchiveResult {
  fileName: string;
  monthAbbrev: string | null;
  totalEntries: number;
  matched: number;
  skippedXml: number;
  ingested: number;
  failedPaths: string[];
  unmatchedPaths: string[];
  status: 'pending' | 'processing' | 'done' | 'error';
}

const BatchIngest: React.FC = () => {
  const navigate = useNavigate();
  const fileInputRef = useRef<HTMLInputElement>(null);
  const [archives, setArchives] = useState<File[]>([]);
  const [results, setResults] = useState<ArchiveResult[]>([]);
  const [processing, setProcessing] = useState(false);

  const handleSelectFiles = (fileList: FileList) => {
    setArchives(Array.from(fileList));
    setResults([]);
  };

  const processArchive = useCallback(async (archive: File, options: DocumentOption[]): Promise<ArchiveResult> => {
    const { entries, monthAbbrev } = await parseZip(archive);
    const { matchesByOptionId, unmatched } = matchEntriesToOptions(entries, options);

    const assignments: { documentOptionId: number; path: string }[] = [];
    let skippedXml = 0;

    for (const [documentOptionId, entry] of matchesByOptionId) {
      if (entry.path.toLowerCase().endsWith('.xml')) {
        skippedXml++;
        continue;
      }
      assignments.push({ documentOptionId, path: entry.path });
    }

    const base: ArchiveResult = {
      fileName: archive.name,
      monthAbbrev,
      totalEntries: entries.length,
      matched: matchesByOptionId.size,
      skippedXml,
      ingested: 0,
      failedPaths: [],
      unmatchedPaths: unmatched.map((e) => e.path),
      status: 'done',
    };

    if (assignments.length === 0) return base;

    const formData = new FormData();
    formData.append('archive', archive);
    formData.append('assignments', JSON.stringify(assignments));

    const res = await fetch(`${API_URL}/documentsample/import-zip`, { method: 'POST', body: formData });
    if (!res.ok) throw new Error(`HTTP ${res.status}`);

    const data: ImportZipResponse = await res.json();
    return {
      ...base,
      ingested: data.ingested,
      failedPaths: data.failed.map((f) => `${f.path} (${f.reason})`),
    };
  }, []);

  const handleStart = useCallback(async () => {
    if (archives.length === 0) return;
    setProcessing(true);

    let options: DocumentOption[];
    try {
      const res = await fetch(`${API_URL}/documentoption`);
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      options = await res.json();
    } catch {
      toast.error('Failed to load document types');
      setProcessing(false);
      return;
    }

    setResults(
      archives.map((f) => ({
        fileName: f.name,
        monthAbbrev: null,
        totalEntries: 0,
        matched: 0,
        skippedXml: 0,
        ingested: 0,
        failedPaths: [],
        unmatchedPaths: [],
        status: 'pending',
      }))
    );

    for (let i = 0; i < archives.length; i++) {
      setResults((prev) => prev.map((r, idx) => (idx === i ? { ...r, status: 'processing' } : r)));

      try {
        const result = await processArchive(archives[i], options);
        setResults((prev) => prev.map((r, idx) => (idx === i ? result : r)));
      } catch {
        setResults((prev) => prev.map((r, idx) => (idx === i ? { ...r, status: 'error' } : r)));
      }
    }

    setProcessing(false);
    toast.success('Batch ingest complete');
  }, [archives, processArchive]);

  const totalIngested = results.reduce((sum, r) => sum + r.ingested, 0);

  return (
    <div className="batch-ingest">
      <div className="batch-ingest__header">
        <button className="batch-ingest__back" onClick={() => navigate('/')} aria-label="Back">
          <ArrowLeftIcon size={20} />
        </button>
        <h1 className="batch-ingest__title">Batch Ingest</h1>
      </div>

      <div className="batch-ingest__intro">
        Select every monthly ZIP you want to backfill into the training corpus at once (e.g. a full
        year). Each archive is matched against the current document types, `.xml` entries are skipped
        (they are classified by CNPJ, not by embedding), and the rest are ingested as labelled samples.
        Unlike the Upload page's Import ZIP, this never touches the Upload screen — nothing here is
        shown for on-screen verification first, so review the per-archive results below afterwards.
      </div>

      <div className="batch-ingest__controls">
        <input
          ref={fileInputRef}
          type="file"
          multiple
          accept=".zip"
          style={{ display: 'none' }}
          onChange={(e) => {
            if (e.target.files && e.target.files.length > 0) handleSelectFiles(e.target.files);
            e.target.value = '';
          }}
        />
        <button
          className="batch-ingest__select-btn"
          onClick={() => fileInputRef.current?.click()}
          disabled={processing}
        >
          <ImportIcon size={16} />
          Choose ZIP files
        </button>
        <span className="batch-ingest__selected-count">
          {archives.length > 0 ? `${archives.length} archive${archives.length === 1 ? '' : 's'} selected` : 'No archives selected'}
        </span>
        <button
          className="batch-ingest__start-btn"
          onClick={handleStart}
          disabled={processing || archives.length === 0}
        >
          {processing ? 'Processing...' : 'Start batch ingest'}
        </button>
      </div>

      {results.length > 0 && (
        <div className="batch-ingest__summary">
          Total ingested so far: <strong>{totalIngested}</strong>
        </div>
      )}

      <div className="batch-ingest__results">
        {results.map((r, i) => (
          <div key={i} className={`batch-ingest__row batch-ingest__row--${r.status}`}>
            <div className="batch-ingest__row-header">
              <span className="batch-ingest__row-name">
                {r.fileName}
                {r.monthAbbrev ? ` (${r.monthAbbrev})` : ''}
              </span>
              <span className="batch-ingest__row-status">
                {r.status === 'pending' && 'Waiting...'}
                {r.status === 'processing' && 'Processing...'}
                {r.status === 'error' && 'Failed to read archive'}
                {r.status === 'done' && `${r.ingested} ingested`}
              </span>
            </div>

            {r.status === 'done' && (
              <div className="batch-ingest__row-details">
                <span>{r.matched} of {r.totalEntries} entries matched</span>
                {r.skippedXml > 0 && <span>{r.skippedXml} XML skipped</span>}
                {r.failedPaths.length > 0 && <span className="batch-ingest__row-warning">{r.failedPaths.length} failed</span>}
                {r.unmatchedPaths.length > 0 && (
                  <span className="batch-ingest__row-warning">{r.unmatchedPaths.length} unmatched</span>
                )}
              </div>
            )}

            {(r.unmatchedPaths.length > 0 || r.failedPaths.length > 0) && (
              <ul className="batch-ingest__row-issues">
                {r.unmatchedPaths.map((p, j) => (
                  <li key={`u-${j}`}>No slot found for: {p}</li>
                ))}
                {r.failedPaths.map((p, j) => (
                  <li key={`f-${j}`}>Failed: {p}</li>
                ))}
              </ul>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default BatchIngest;
