import React from 'react';
import { EyeIcon } from '../../../components/icons';
import type { DocumentSampleDetail } from '../../../types';
import './styles.css';

interface SampleTextViewerProps {
  isOpen: boolean;
  onClose: () => void;
  loading: boolean;
  sample: DocumentSampleDetail | null;
}

const SampleTextViewer: React.FC<SampleTextViewerProps> = ({ isOpen, onClose, loading, sample }) => {
  if (!isOpen) return null;

  return (
    <div className="sample-text-viewer-backdrop" onClick={onClose}>
      <div className="sample-text-viewer-dialog" onClick={(e) => e.stopPropagation()}>
        <div className="sample-text-viewer-header">
          <div className="sample-text-viewer-title">
            <EyeIcon size={20} />
            <h2>{sample ? sample.sourceFileName : 'Sample'}</h2>
          </div>
          <button onClick={onClose} className="sample-text-viewer-close" aria-label="Close">
            ✕
          </button>
        </div>

        <div className="sample-text-viewer-content">
          {loading ? (
            <div className="sample-text-viewer-loading">Loading...</div>
          ) : (
            <pre className="sample-text-viewer-text">{sample?.extractedText}</pre>
          )}
        </div>

        <div className="sample-text-viewer-footer">
          <button onClick={onClose} className="sample-text-viewer-btn">
            Close
          </button>
        </div>
      </div>
    </div>
  );
};

export default SampleTextViewer;
