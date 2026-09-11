import React, { useState } from 'react';
import { useDocumentManager } from '../../hooks/useDocumentManager';
import MenuBar from '../../components/features/MenuBar';
import FileList from '../../components/features/FileList';
import FilePreview from '../../components/features/FilePreview';
import Toolbar from '../../components/features/Toolbar';
import SendEmail from '../../components/features/SendEmail';
import ClassificationDetails from './ClassificationDetails';
import './styles.css';

const Upload: React.FC = () => {
  const {
    files,
    loading,
    classifying,
    importing,
    ingesting,
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
    ingestSamples,
  } = useDocumentManager();

  const [sendEmailOpen, setSendEmailOpen] = useState(false);
  const [classificationDetailsOpen, setClassificationDetailsOpen] = useState(false);
  const filledCount = files.filter((f) => f.file !== null).length;
  const hasClassifications = files.some((f) => f.classification !== undefined);

  return (
    <div className="upload-page">
      <MenuBar />
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
            onIngestSamples={ingestSamples}
            onShowClassificationDetails={() => setClassificationDetailsOpen(true)}
            classifying={classifying}
            importing={importing}
            ingesting={ingesting}
            hasClassifications={hasClassifications}
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
      {classificationDetailsOpen && (
        <ClassificationDetails
          isOpen={classificationDetailsOpen}
          onClose={() => setClassificationDetailsOpen(false)}
          files={files}
        />
      )}
    </div>
  );
};

export default Upload;
