import React, { useCallback, useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { ArrowLeftIcon, EyeIcon, TrashIcon, UploadIcon } from '../../components/icons';
import SampleTextViewer from './SampleTextViewer';
import ConfirmDialog from './ConfirmDialog';
import { API_URL } from '../../lib/api';
import type { DocumentOptionItem, DocumentSampleDetail, DocumentSampleGroup } from '../../types';
import './styles.css';

const LOW_SAMPLE_COUNT_THRESHOLD = 10;

type DeleteTarget = { ids: number[]; title: string; message: string };

const SamplesManagement: React.FC = () => {
  const navigate = useNavigate();
  const [options, setOptions] = useState<DocumentOptionItem[]>([]);
  const [groups, setGroups] = useState<DocumentSampleGroup[]>([]);
  const [loading, setLoading] = useState(true);
  const [uploading, setUploading] = useState(false);
  const [selectedOptionId, setSelectedOptionId] = useState<number | ''>('');
  const fileInputRef = useRef<HTMLInputElement>(null);
  const [viewerOpen, setViewerOpen] = useState(false);
  const [viewerLoading, setViewerLoading] = useState(false);
  const [viewedSample, setViewedSample] = useState<DocumentSampleDetail | null>(null);
  const [selectedIds, setSelectedIds] = useState<Set<number>>(new Set());
  const [deleteTarget, setDeleteTarget] = useState<DeleteTarget | null>(null);

  const loadGroups = useCallback(async () => {
    const res = await fetch(`${API_URL}/documentsample`);
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    setGroups(await res.json());
  }, []);

  useEffect(() => {
    Promise.all([
      fetch(`${API_URL}/documentoption`).then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json() as Promise<DocumentOptionItem[]>;
      }),
      fetch(`${API_URL}/documentsample`).then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json() as Promise<DocumentSampleGroup[]>;
      }),
    ])
      .then(([optionsData, groupsData]) => {
        setOptions(optionsData);
        setGroups(groupsData);
      })
      .catch(() => toast.error('Failed to load training samples'))
      .finally(() => setLoading(false));
  }, []);

  const handleUpload = useCallback(
    async (fileList: FileList) => {
      if (!selectedOptionId) {
        toast.warning('Choose a document type before uploading samples.');
        return;
      }

      const toastId = toast.loading('Uploading samples...');
      setUploading(true);

      try {
        const formData = new FormData();
        formData.append('documentOptionId', String(selectedOptionId));
        Array.from(fileList).forEach((file) => formData.append('files', file));

        const res = await fetch(`${API_URL}/documentsample`, { method: 'POST', body: formData });
        if (!res.ok) throw new Error(`HTTP ${res.status}`);

        await loadGroups();
        toast.update(toastId, { render: 'Samples uploaded', type: 'success', isLoading: false, autoClose: 3000 });
      } catch {
        toast.update(toastId, { render: 'Failed to upload samples', type: 'error', isLoading: false, autoClose: 3000 });
      } finally {
        setUploading(false);
      }
    },
    [selectedOptionId, loadGroups]
  );

  const toggleSelected = useCallback((id: number) => {
    setSelectedIds((prev) => {
      const next = new Set(prev);
      if (next.has(id)) next.delete(id);
      else next.add(id);
      return next;
    });
  }, []);

  const requestDeleteOne = useCallback((id: number, fileName: string) => {
    setDeleteTarget({
      ids: [id],
      title: 'Remove sample?',
      message: `This will permanently delete "${fileName}". This cannot be undone.`,
    });
  }, []);

  const requestDeleteSelected = useCallback(() => {
    setDeleteTarget({
      ids: Array.from(selectedIds),
      title: 'Delete selected samples?',
      message: `This will permanently delete ${selectedIds.size} selected sample${selectedIds.size === 1 ? '' : 's'}. This cannot be undone.`,
    });
  }, [selectedIds]);

  const confirmDelete = useCallback(async () => {
    if (!deleteTarget) return;
    const { ids } = deleteTarget;
    setDeleteTarget(null);

    const results = await Promise.allSettled(
      ids.map((id) => fetch(`${API_URL}/documentsample/${id}`, { method: 'DELETE' }).then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
      }))
    );

    const failed = results.filter((r) => r.status === 'rejected').length;
    const succeeded = ids.length - failed;

    setSelectedIds((prev) => {
      const next = new Set(prev);
      ids.forEach((id) => next.delete(id));
      return next;
    });

    await loadGroups();

    if (failed === 0)
      toast.success(succeeded === 1 ? 'Sample removed' : `${succeeded} samples removed`);
    else
      toast.warning(`${succeeded} of ${ids.length} samples removed; ${failed} failed`);
  }, [deleteTarget, loadGroups]);

  const handleView = useCallback(async (id: number) => {
    setViewerOpen(true);
    setViewerLoading(true);
    setViewedSample(null);

    try {
      const res = await fetch(`${API_URL}/documentsample/${id}`);
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      setViewedSample(await res.json());
    } catch {
      toast.error('Failed to load sample');
      setViewerOpen(false);
    } finally {
      setViewerLoading(false);
    }
  }, []);

  return (
    <div className="samples-management">
      <div className="samples-management__header">
        <button className="samples-management__back" onClick={() => navigate('/')} aria-label="Back">
          <ArrowLeftIcon size={20} />
        </button>
        <h1 className="samples-management__title">Training Samples</h1>
      </div>

      <div className="samples-management__upload">
        <select
          className="samples-management__select"
          value={selectedOptionId}
          onChange={(e) => setSelectedOptionId(e.target.value ? Number(e.target.value) : '')}
        >
          <option value="">Choose document type...</option>
          {options.map((option) => (
            <option key={option.id} value={option.id}>
              {option.description}
            </option>
          ))}
        </select>
        <input
          ref={fileInputRef}
          type="file"
          multiple
          accept=".pdf,.jpg,.jpeg,.png"
          style={{ display: 'none' }}
          onChange={(e) => {
            if (e.target.files && e.target.files.length > 0) handleUpload(e.target.files);
            e.target.value = '';
          }}
        />
        <button
          className="samples-management__upload-btn"
          onClick={() => fileInputRef.current?.click()}
          disabled={uploading || !selectedOptionId}
        >
          <UploadIcon size={16} />
          {uploading ? 'Uploading...' : 'Add samples'}
        </button>
        <button
          className="samples-management__delete-selected-btn"
          onClick={requestDeleteSelected}
          disabled={selectedIds.size === 0}
        >
          <TrashIcon size={16} />
          Delete selected {selectedIds.size > 0 ? `(${selectedIds.size})` : ''}
        </button>
      </div>

      {loading ? (
        <div className="samples-management__loading">Loading...</div>
      ) : groups.length === 0 ? (
        <div className="samples-management__empty">
          No samples ingested yet. Add some above, or use "Ingest as samples" on the Upload screen.
        </div>
      ) : (
        <div className="samples-management__groups">
          {groups.map((group) => (
            <div key={group.documentOptionId} className="samples-management__group">
              <div className="samples-management__group-header">
                <span className="samples-management__group-title">{group.documentOptionDescription}</span>
                <span
                  className={`samples-management__count ${
                    group.count < LOW_SAMPLE_COUNT_THRESHOLD ? 'samples-management__count--low' : ''
                  }`}
                >
                  {group.count} sample{group.count === 1 ? '' : 's'}
                </span>
              </div>
              <ul className="samples-management__list">
                {group.samples.map((sample) => (
                  <li key={sample.id} className="samples-management__item">
                    <input
                      type="checkbox"
                      className="samples-management__checkbox"
                      checked={selectedIds.has(sample.id)}
                      onChange={() => toggleSelected(sample.id)}
                      aria-label={`Select ${sample.sourceFileName}`}
                    />
                    <span className="samples-management__item-name">{sample.sourceFileName}</span>
                    <div className="samples-management__item-actions">
                      <button
                        className="samples-management__view-btn"
                        onClick={() => handleView(sample.id)}
                        aria-label={`View ${sample.sourceFileName}`}
                      >
                        <EyeIcon size={14} />
                      </button>
                      <button
                        className="samples-management__delete-btn"
                        onClick={() => requestDeleteOne(sample.id, sample.sourceFileName)}
                        aria-label={`Remove ${sample.sourceFileName}`}
                      >
                        <TrashIcon size={14} color="var(--status-required)" />
                      </button>
                    </div>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      )}

      <SampleTextViewer
        isOpen={viewerOpen}
        onClose={() => setViewerOpen(false)}
        loading={viewerLoading}
        sample={viewedSample}
      />

      <ConfirmDialog
        isOpen={deleteTarget !== null}
        title={deleteTarget?.title ?? ''}
        message={deleteTarget?.message ?? ''}
        confirmLabel="Delete"
        onConfirm={confirmDelete}
        onCancel={() => setDeleteTarget(null)}
      />
    </div>
  );
};

export default SamplesManagement;
