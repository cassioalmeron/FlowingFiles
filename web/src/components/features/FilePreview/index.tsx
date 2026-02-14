import React, { useEffect, useState } from 'react';
import type { FileEntry } from '../../../types';
import { EyeIcon, FileIcon } from '../../icons';
import './styles.css';

interface FilePreviewProps {
  entry: FileEntry | null;
}

function getFileType(name: string): 'pdf' | 'image' | 'text' | 'unknown' {
  const ext = name.split('.').pop()?.toLowerCase() ?? '';
  if (ext === 'pdf') return 'pdf';
  if (['png', 'jpg', 'jpeg', 'gif', 'bmp', 'webp', 'svg'].includes(ext)) return 'image';
  if (['xml', 'ofx', 'txt', 'csv', 'json', 'html'].includes(ext)) return 'text';
  return 'unknown';
}

const FilePreview: React.FC<FilePreviewProps> = ({ entry }) => {
  const [objectUrl, setObjectUrl] = useState<string | null>(null);
  const [textContent, setTextContent] = useState<string | null>(null);

  useEffect(() => {
    if (!entry?.file) {
      setObjectUrl(null);
      setTextContent(null);
      return;
    }

    const file = entry.file;
    const type = getFileType(file.name);

    if (type === 'pdf' || type === 'image') {
      const url = URL.createObjectURL(file);
      setObjectUrl(url);
      setTextContent(null);
      return () => URL.revokeObjectURL(url);
    }

    if (type === 'text') {
      setObjectUrl(null);
      const reader = new FileReader();
      reader.onload = () => setTextContent(reader.result as string);
      reader.readAsText(file);
      return;
    }

    setObjectUrl(null);
    setTextContent(null);
  }, [entry?.file]);

  if (!entry?.file) {
    return (
      <div className="file-preview file-preview--empty">
        <EyeIcon size={48} color="var(--bg-7)" />
        <p>Select a file to preview</p>
      </div>
    );
  }

  const file = entry.file;
  const type = getFileType(file.name);

  if (type === 'pdf' && objectUrl) {
    return (
      <div className="file-preview">
        <iframe className="file-preview__iframe" src={objectUrl} title="PDF preview" />
      </div>
    );
  }

  if (type === 'image' && objectUrl) {
    return (
      <div className="file-preview file-preview--centered">
        <img className="file-preview__image" src={objectUrl} alt={file.name} />
      </div>
    );
  }

  if (type === 'text' && textContent !== null) {
    return (
      <div className="file-preview">
        <pre className="file-preview__text">{textContent}</pre>
      </div>
    );
  }

  return (
    <div className="file-preview file-preview--empty">
      <FileIcon size={48} color="var(--bg-7)" />
      <p>{file.name}</p>
      <span className="file-preview__hint">Preview not available for this file type</span>
    </div>
  );
};

export default FilePreview;
