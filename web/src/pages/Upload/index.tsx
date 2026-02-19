import React from 'react';
import { useDocumentManager } from '../../hooks/useDocumentManager';
import MenuBar from '../../components/features/MenuBar';
import FileList from '../../components/features/FileList';
import FilePreview from '../../components/features/FilePreview';
import Toolbar from '../../components/features/Toolbar';
import './styles.css';

const Upload: React.FC = () => {
  const {
    files,
    loading,
    currentIndex,
    currentFile,
    selectedMonth,
    setCurrentIndex,
    setSelectedMonth,
    selectFile,
    clearFile,
    exportZip,
  } = useDocumentManager();

  const filledCount = files.filter((f) => f.file !== null).length;

  return (
    <div className="upload-page">
      <MenuBar onExportZip={exportZip} />
      <div className="upload-page__body">
        <div className="upload-page__sidebar">
          <FileList
            files={files}
            loading={loading}
            currentIndex={currentIndex}
            onSelect={setCurrentIndex}
            onFileChange={selectFile}
            onClear={clearFile}
          />
        </div>
        <div className="upload-page__main">
          <FilePreview entry={currentFile} />
          <Toolbar
            selectedMonth={selectedMonth}
            onMonthChange={setSelectedMonth}
            onExportZip={exportZip}
            filledCount={filledCount}
            totalCount={files.length}
          />
        </div>
      </div>
    </div>
  );
};

export default Upload;
