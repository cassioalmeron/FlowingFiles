import React from 'react';
import type { FileEntry } from '../../../types';
import { InfoIcon } from '../../../components/icons';
import './styles.css';

interface ClassificationDetailsProps {
  isOpen: boolean;
  onClose: () => void;
  files: FileEntry[];
}

const ClassificationDetails: React.FC<ClassificationDetailsProps> = ({ isOpen, onClose, files }) => {
  if (!isOpen) return null;

  const attached = files.filter((entry) => entry.file !== null);

  return (
    <div className="classification-details-backdrop" onClick={onClose}>
      <div className="classification-details-dialog" onClick={(e) => e.stopPropagation()}>
        <div className="classification-details-header">
          <div className="classification-details-title">
            <InfoIcon size={20} />
            <h2>Classification Details</h2>
          </div>
          <button onClick={onClose} className="classification-details-close" aria-label="Close">
            ✕
          </button>
        </div>

        <div className="classification-details-content">
          {attached.length === 0 ? (
            <div className="classification-details-empty">No files attached yet.</div>
          ) : (
            attached.map((entry, index) => (
              <div key={index} className="classification-details-row">
                <div className="classification-details-row-header">
                  <div className="classification-details-file">
                    <span className="classification-details-slot">{entry.option.description}</span>
                    <span className="classification-details-filename">{entry.file!.name}</span>
                  </div>
                  {entry.classification ? (
                    <span
                      className={`classification-details-badge classification-details-badge--${entry.classification.method.toLowerCase()}`}
                    >
                      {entry.classification.method === 'Rule' ? 'Rule match' : 'Similarity'}
                    </span>
                  ) : (
                    <span className="classification-details-badge classification-details-badge--none">
                      Not auto-classified
                    </span>
                  )}
                </div>

                {entry.classification?.method === 'Similarity' && entry.classification.neighbours && (
                  <ul className="classification-details-neighbours">
                    {entry.classification.neighbours.map((neighbour, i) => (
                      <li
                        key={i}
                        className={`classification-details-neighbour ${
                          neighbour.label === entry.classification!.label
                            ? 'classification-details-neighbour--best'
                            : ''
                        }`}
                      >
                        <span>{neighbour.label}</span>
                        <span className="classification-details-score">
                          {(neighbour.similarity * 100).toFixed(1)}%
                        </span>
                      </li>
                    ))}
                  </ul>
                )}
              </div>
            ))
          )}
        </div>

        <div className="classification-details-footer">
          <button onClick={onClose} className="classification-details-btn">
            Close
          </button>
        </div>
      </div>
    </div>
  );
};

export default ClassificationDetails;
