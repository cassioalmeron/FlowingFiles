import React, { useRef } from 'react';
import { getFileStatus, FileStatus } from '../../../types';
import type { FileEntry } from '../../../types';
import { UploadIcon, TrashIcon } from '../../icons';
import './styles.css';

interface FileListItemProps {
  entry: FileEntry;
  index: number;
  isSelected: boolean;
  onSelect: (index: number) => void;
  onFileChange: (index: number, file: File) => void;
  onClear: (index: number) => void;
}

const STATUS_COLORS: Record<FileStatus, string> = {
  [FileStatus.Filled]: 'var(--status-filled)',
  [FileStatus.EmptyRequired]: 'var(--status-required)',
  [FileStatus.EmptyOptional]: 'var(--status-optional)',
};

const FileListItem: React.FC<FileListItemProps> = ({
  entry,
  index,
  isSelected,
  onSelect,
  onFileChange,
  onClear,
}) => {
  const inputRef = useRef<HTMLInputElement>(null);
  const status = getFileStatus(entry);

  const handleFileInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      onFileChange(index, file);
    }
    e.target.value = '';
  };

  return (
    <div
      className={`file-list-item ${isSelected ? 'file-list-item--selected' : ''}`}
      style={{ borderLeftColor: STATUS_COLORS[status] }}
      onClick={() => onSelect(index)}
    >
      <div className="file-list-item__header">
        <span className="file-list-item__description">{entry.option.description}</span>
        {!entry.option.required && <span className="file-list-item__optional">optional</span>}
      </div>

      <div className="file-list-item__row">
        <span className="file-list-item__filename">
          {entry.file ? entry.file.name : 'No file selected'}
        </span>

        <div className="file-list-item__actions">
          <button
            className="file-list-item__btn"
            onClick={(e) => { e.stopPropagation(); inputRef.current?.click(); }}
            title="Choose file"
          >
            <UploadIcon size={16} />
          </button>

          {entry.file && (
            <button
              className="file-list-item__btn file-list-item__btn--danger"
              onClick={(e) => { e.stopPropagation(); onClear(index); }}
              title="Clear file"
            >
              <TrashIcon size={16} />
            </button>
          )}
        </div>
      </div>

      <input
        ref={inputRef}
        type="file"
        style={{ display: 'none' }}
        onChange={handleFileInput}
      />
    </div>
  );
};

export default FileListItem;
