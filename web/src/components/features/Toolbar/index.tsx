import React from 'react';
import { AutoClassifyIcon, DownloadIcon, ImportIcon, SendIcon } from '../../icons';
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
  classifying: boolean;
  importing: boolean;
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
  classifying,
  importing,
  filledCount,
  totalCount,
  fileInputRef,
  zipInputRef,
}) => {
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
        <button
          className="toolbar__import-btn"
          onClick={() => zipInputRef.current?.click()}
          disabled={importing}
        >
          <ImportIcon size={16} />
          {importing ? 'Loading...' : 'Import ZIP'}
        </button>
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
      </div>
    </div>
  );
};

export default Toolbar;
