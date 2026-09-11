import React, { useEffect, useRef, useState } from 'react';
import { AutoClassifyIcon, DownloadIcon, ImportIcon, InfoIcon, LayersIcon, MoreIcon, SendIcon } from '../../icons';
import './styles.css';

const MONTHS = [
  'January', 'February', 'March', 'April', 'May', 'June',
  'July', 'August', 'September', 'October', 'November', 'December',
];

interface ToolbarProps {
  selectedMonth: number;
  onMonthChange: (month: number) => void;
  onExportZip: () => void;
  onSendEmail: () => void;
  onAutoClassify: (files: FileList) => void;
  onImportZip: (archive: File) => void;
  onIngestSamples: () => void;
  onShowClassificationDetails: () => void;
  classifying: boolean;
  importing: boolean;
  ingesting: boolean;
  hasClassifications: boolean;
  filledCount: number;
  totalCount: number;
  fileInputRef: React.RefObject<HTMLInputElement | null>;
  zipInputRef: React.RefObject<HTMLInputElement | null>;
}

const Toolbar: React.FC<ToolbarProps> = ({
  selectedMonth,
  onMonthChange,
  onExportZip,
  onSendEmail,
  onAutoClassify,
  onImportZip,
  onIngestSamples,
  onShowClassificationDetails,
  classifying,
  importing,
  ingesting,
  hasClassifications,
  filledCount,
  totalCount,
  fileInputRef,
  zipInputRef,
}) => {
  const [moreMenuOpen, setMoreMenuOpen] = useState(false);
  const moreMenuRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!moreMenuOpen) return;

    const handleClickOutside = (e: MouseEvent) => {
      if (moreMenuRef.current && !moreMenuRef.current.contains(e.target as Node))
        setMoreMenuOpen(false);
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [moreMenuOpen]);

  return (
    <div className="toolbar">
      <div className="toolbar__left">
        <label className="toolbar__label" htmlFor="month-select">
          Month:
        </label>
        <select
          id="month-select"
          className="toolbar__select"
          value={selectedMonth}
          onChange={(e) => onMonthChange(Number(e.target.value))}
        >
          {MONTHS.map((name, i) => (
            <option key={i + 1} value={i + 1}>
              {name}
            </option>
          ))}
        </select>
      </div>

      <div className="toolbar__right">
        <span className="toolbar__status">
          {filledCount} / {totalCount} files
        </span>
        <input
          ref={zipInputRef}
          type="file"
          accept=".zip"
          style={{ display: 'none' }}
          onChange={(e) => {
            const archive = e.target.files?.[0];
            if (archive) onImportZip(archive);
            e.target.value = '';
          }}
        />
        <input
          ref={fileInputRef}
          type="file"
          multiple
          accept=".pdf,.jpg,.jpeg,.png,.xml"
          style={{ display: 'none' }}
          onChange={(e) => e.target.files && onAutoClassify(e.target.files)}
        />
        <button
          className="toolbar__classify-btn"
          onClick={() => fileInputRef.current?.click()}
          disabled={classifying}
        >
          <AutoClassifyIcon size={16} />
          {classifying ? 'Classifying...' : 'Auto-classify'}
        </button>
        <button
          className="toolbar__info-btn"
          onClick={onShowClassificationDetails}
          disabled={!hasClassifications}
          title="View classification details for every attached file"
          aria-label="Classification details"
        >
          <InfoIcon size={16} />
        </button>
        <button
          className="toolbar__send-btn"
          onClick={onSendEmail}
          disabled={filledCount === 0}
        >
          <SendIcon size={16} />
          Send Email
        </button>
        <button
          className="toolbar__export-btn"
          onClick={onExportZip}
          disabled={filledCount === 0}
        >
          <DownloadIcon size={16} />
          Export as ZIP
        </button>

        <div className="toolbar__more" ref={moreMenuRef}>
          <button
            className="toolbar__more-btn"
            onClick={() => setMoreMenuOpen((prev) => !prev)}
            title="More actions"
            aria-label="More actions"
            aria-expanded={moreMenuOpen}
          >
            <MoreIcon size={18} />
          </button>

          {moreMenuOpen && (
            <div className="toolbar__more-menu">
              <button
                className="toolbar__more-menu-item"
                onClick={() => {
                  setMoreMenuOpen(false);
                  zipInputRef.current?.click();
                }}
                disabled={importing}
              >
                <ImportIcon size={16} />
                {importing ? 'Loading...' : 'Import ZIP'}
              </button>
              <button
                className="toolbar__more-menu-item"
                onClick={() => {
                  setMoreMenuOpen(false);
                  onIngestSamples();
                }}
                disabled={ingesting || filledCount === 0}
                title="Save the attached files as labelled training samples for the similarity classifier"
              >
                <LayersIcon size={16} />
                {ingesting ? 'Ingesting...' : 'Ingest as samples'}
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Toolbar;
