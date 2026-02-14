import React from 'react';
import type { FileEntry } from '../../../types';
import FileListItem from '../FileListItem';
import './styles.css';

interface FileListProps {
  files: FileEntry[];
  currentIndex: number | null;
  onSelect: (index: number) => void;
  onFileChange: (index: number, file: File) => void;
  onClear: (index: number) => void;
}

const FileList: React.FC<FileListProps> = ({
  files,
  currentIndex,
  onSelect,
  onFileChange,
  onClear,
}) => {
  return (
    <div className="file-list">
      <div className="file-list__header">
        <h2 className="file-list__title">Documents</h2>
        <span className="file-list__count">
          {files.filter((f) => f.file !== null).length} / {files.length}
        </span>
      </div>
      <div className="file-list__items">
        {files.map((entry, index) => (
          <FileListItem
            key={index}
            entry={entry}
            index={index}
            isSelected={currentIndex === index}
            onSelect={onSelect}
            onFileChange={onFileChange}
            onClear={onClear}
          />
        ))}
      </div>
    </div>
  );
};

export default FileList;
