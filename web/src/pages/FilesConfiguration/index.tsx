import React, { useState, useCallback, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { DragDropContext, Droppable, Draggable, type DropResult } from '@hello-pangea/dnd';
import { toast } from 'react-toastify';
import type { DocumentOptionItem } from '../../types';
import { API_URL } from '../../lib/api';
import { ArrowLeftIcon, GripIcon, PlusIcon, TrashIcon } from '../../components/icons';
import './styles.css';

const FilesConfiguration: React.FC = () => {
  const navigate = useNavigate();
  const [documents, setDocuments] = useState<DocumentOptionItem[]>([]);
  const [savedSnapshot, setSavedSnapshot] = useState<string>('[]');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch(`${API_URL}/documentoption`)
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json() as Promise<DocumentOptionItem[]>;
      })
      .then((data) => {
        setDocuments(data);
        setSavedSnapshot(JSON.stringify(data));
      })
      .catch(() => {
        toast.error('Failed to load document options');
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  const hasChanges = JSON.stringify(documents) !== savedSnapshot;

  const addDocument = useCallback(() => {
    setDocuments((prev) => {
      const newItem: DocumentOptionItem = {
        id: Date.now() * -1,
        description: '',
        path: '',
        required: false,
        position: prev.length + 1,
      };
      return [...prev, newItem];
    });
  }, []);

  const updateDocument = useCallback(
    (id: number, field: keyof DocumentOptionItem, value: string | boolean) => {
      setDocuments((prev) =>
        prev.map((doc) => (doc.id === id ? { ...doc, [field]: value } : doc))
      );
    },
    []
  );

  const removeDocument = useCallback((id: number) => {
    const doc = documents.find((d) => d.id === id);
    const label = doc?.description || 'this item';

    toast(
      ({ closeToast }) => (
        <div>
          <p>Remove "{label}"?</p>
          <div style={{ display: 'flex', gap: '8px', marginTop: '8px' }}>
            <button
              onClick={() => {
                setDocuments((prev) => {
                  const filtered = prev.filter((d) => d.id !== id);
                  return filtered.map((d, i) => ({ ...d, position: i + 1 }));
                });
                closeToast?.();
                toast.success('Item removed');
              }}
              style={{
                padding: '4px 12px',
                background: 'var(--status-required)',
                color: '#fff',
                border: 'none',
                borderRadius: '4px',
                cursor: 'pointer',
              }}
            >
              Remove
            </button>
            <button
              onClick={() => closeToast?.()}
              style={{
                padding: '4px 12px',
                background: 'var(--bg-6)',
                color: 'var(--fg-primary)',
                border: 'none',
                borderRadius: '4px',
                cursor: 'pointer',
              }}
            >
              Cancel
            </button>
          </div>
        </div>
      ),
      { autoClose: false, closeOnClick: false }
    );
  }, [documents]);

  const handleDragEnd = useCallback((result: DropResult) => {
    if (!result.destination) return;

    setDocuments((prev) => {
      const items = [...prev];
      const [moved] = items.splice(result.source.index, 1);
      items.splice(result.destination!.index, 0, moved);
      return items.map((item, i) => ({ ...item, position: i + 1 }));
    });
  }, []);

  const handleSave = useCallback(async () => {
    // Validation
    const emptyDescriptions = documents.filter((d) => !d.description.trim());
    if (emptyDescriptions.length > 0) {
      toast.error('All items must have a description');
      return;
    }

    const emptyPaths = documents.filter((d) => !d.path.trim());
    if (emptyPaths.length > 0) {
      toast.error('All items must have a path');
      return;
    }

    const descriptions = documents.map((d) => d.description.trim().toLowerCase());
    const duplicates = descriptions.filter((d, i) => descriptions.indexOf(d) !== i);
    if (duplicates.length > 0) {
      toast.error(`Duplicate description: "${duplicates[0]}"`);
      return;
    }

    try {
      const res = await fetch(`${API_URL}/documentoption`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(documents.map((d) => ({ ...d, id: d.id < 0 ? 0 : d.id }))),
      });

      if (!res.ok) throw new Error(`HTTP ${res.status}`);

      const saved = (await res.json()) as DocumentOptionItem[];
      setDocuments(saved);
      setSavedSnapshot(JSON.stringify(saved));
      toast.success('Configuration saved');
    } catch {
      toast.error('Failed to save configuration');
    }
  }, [documents]);

  const handleBack = useCallback(() => {
    if (hasChanges) {
      toast(
        ({ closeToast }) => (
          <div>
            <p>You have unsaved changes. Discard?</p>
            <div style={{ display: 'flex', gap: '8px', marginTop: '8px' }}>
              <button
                onClick={() => {
                  closeToast?.();
                  navigate('/');
                }}
                style={{
                  padding: '4px 12px',
                  background: 'var(--status-required)',
                  color: '#fff',
                  border: 'none',
                  borderRadius: '4px',
                  cursor: 'pointer',
                }}
              >
                Discard
              </button>
              <button
                onClick={() => closeToast?.()}
                style={{
                  padding: '4px 12px',
                  background: 'var(--bg-6)',
                  color: 'var(--fg-primary)',
                  border: 'none',
                  borderRadius: '4px',
                  cursor: 'pointer',
                }}
              >
                Stay
              </button>
            </div>
          </div>
        ),
        { autoClose: false, closeOnClick: false }
      );
    } else {
      navigate('/');
    }
  }, [hasChanges, navigate]);

  return (
    <div className="files-config">
      <div className="files-config__header">
        <button className="files-config__back" onClick={handleBack} aria-label="Back">
          <ArrowLeftIcon size={20} />
        </button>
        <h1 className="files-config__title">Files Configuration</h1>
        {hasChanges && <span className="files-config__unsaved">Unsaved changes</span>}
      </div>

      <div className="files-config__content">
        {loading ? (
          <div className="files-config__loading">Loading...</div>
        ) : (
          <div className="files-config__table-wrapper">
            <table className="files-config__table">
              <thead>
                <tr>
                  <th className="files-config__th files-config__th--grip"></th>
                  <th className="files-config__th files-config__th--description">Description</th>
                  <th className="files-config__th files-config__th--path">Path</th>
                  <th className="files-config__th files-config__th--required">Required</th>
                  <th className="files-config__th files-config__th--actions"></th>
                </tr>
              </thead>
              <DragDropContext onDragEnd={handleDragEnd}>
                <Droppable droppableId="documents">
                  {(provided) => (
                    <tbody ref={provided.innerRef} {...provided.droppableProps}>
                      {documents.map((doc, index) => (
                        <Draggable key={doc.id} draggableId={String(doc.id)} index={index}>
                          {(provided, snapshot) => (
                            <tr
                              ref={provided.innerRef}
                              {...provided.draggableProps}
                              className={`files-config__row ${snapshot.isDragging ? 'files-config__row--dragging' : ''}`}
                            >
                              <td className="files-config__td files-config__td--grip">
                                <span {...provided.dragHandleProps} className="files-config__drag-handle">
                                  <GripIcon size={16} color="var(--fg-disabled)" />
                                </span>
                              </td>
                              <td className="files-config__td files-config__td--description">
                                <input
                                  type="text"
                                  className="files-config__input"
                                  value={doc.description}
                                  onChange={(e) => updateDocument(doc.id, 'description', e.target.value)}
                                  placeholder="Description"
                                />
                              </td>
                              <td className="files-config__td files-config__td--path">
                                <input
                                  type="text"
                                  className="files-config__input"
                                  value={doc.path}
                                  onChange={(e) => updateDocument(doc.id, 'path', e.target.value)}
                                  placeholder="Path"
                                />
                              </td>
                              <td className="files-config__td files-config__td--required">
                                <input
                                  type="checkbox"
                                  className="files-config__checkbox"
                                  checked={doc.required}
                                  onChange={(e) => updateDocument(doc.id, 'required', e.target.checked)}
                                />
                              </td>
                              <td className="files-config__td files-config__td--actions">
                                <button
                                  className="files-config__delete-btn"
                                  onClick={() => removeDocument(doc.id)}
                                  aria-label={`Remove ${doc.description}`}
                                >
                                  <TrashIcon size={16} color="var(--status-required)" />
                                </button>
                              </td>
                            </tr>
                          )}
                        </Draggable>
                      ))}
                      {provided.placeholder}
                    </tbody>
                  )}
                </Droppable>
              </DragDropContext>
            </table>

            {documents.length === 0 && (
              <div className="files-config__empty">
                No documents configured. Click "Add New File" to get started.
              </div>
            )}
          </div>
        )}
      </div>

      <div className="files-config__actions">
        <button className="files-config__add-btn" onClick={addDocument} disabled={loading}>
          <PlusIcon size={16} />
          Add New File
        </button>
        <button className="files-config__save-btn" onClick={handleSave} disabled={!hasChanges || loading}>
          Save
        </button>
      </div>
    </div>
  );
};

export default FilesConfiguration;
