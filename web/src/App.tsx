import React from 'react';
import { useDocumentManager } from './hooks/useDocumentManager';
import MenuBar from './components/features/MenuBar';
import FileList from './components/features/FileList';
import FilePreview from './components/features/FilePreview';
import Toolbar from './components/features/Toolbar';
import './App.css';

const App: React.FC = () => {
  const {
    files,
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
    <div className="app">
      <MenuBar onExportZip={exportZip} />
      <div className="app__body">
        <div className="app__sidebar">
          <FileList
            files={files}
            currentIndex={currentIndex}
            onSelect={setCurrentIndex}
            onFileChange={selectFile}
            onClear={clearFile}
          />
        </div>
        <div className="app__main">
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

export default App;
