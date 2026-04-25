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
    currentIndex,
    currentFile,
    selectedMonth,
    monthAbbrev,
    fileInputRef,
    setCurrentIndex,
    setSelectedMonth,
    selectFile,
    clearFile,
    exportZip,
    autoClassify,
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
            classifying={classifying}
            filledCount={filledCount}
            totalCount={files.length}
            fileInputRef={fileInputRef}
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
