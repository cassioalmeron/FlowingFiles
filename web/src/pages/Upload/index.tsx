import React, { useState } from 'react';
import { useDocumentManager } from '../../hooks/useDocumentManager';
import MenuBar from '../../components/features/MenuBar';
import FileList from '../../components/features/FileList';
import FilePreview from '../../components/features/FilePreview';
import Toolbar from '../../components/features/Toolbar';
import SendEmail from '../../components/features/SendEmail';
import './styles.css';

const Upload: React.FC = () => {
  const {
    files,
    loading,
    classifying,
    importing,
    currentIndex,
    currentFile,
    selectedMonth,
    monthAbbrev,
    fileInputRef,
    zipInputRef,
    setCurrentIndex,
    setSelectedMonth,
    selectFile,
    clearFile,
    exportZip,
    autoClassify,
    importZip,
  } = useDocumentManager();

  const [sendEmailOpen, setSendEmailOpen] = useState(false);
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
            onSendEmail={() => setSendEmailOpen(true)}
            onAutoClassify={autoClassify}
            onImportZip={importZip}
            classifying={classifying}
            importing={importing}
            filledCount={filledCount}
            totalCount={files.length}
            fileInputRef={fileInputRef}
            zipInputRef={zipInputRef}
          />
        </div>
      </div>
      {sendEmailOpen && (
        <SendEmail
          isOpen={sendEmailOpen}
          onClose={() => setSendEmailOpen(false)}
          selectedMonth={selectedMonth}
          selectedYear={new Date().getFullYear()}
          files={files}
          monthAbbrev={monthAbbrev}
        />
      )}
    </div>
  );
};

export default Upload;
