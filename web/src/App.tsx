import React from 'react';
import { Routes, Route } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Upload from './pages/Upload';
import FilesConfiguration from './pages/FilesConfiguration';
import EmailRegistration from './pages/EmailRegistration';
import SamplesManagement from './pages/SamplesManagement';
import BatchIngest from './pages/BatchIngest';

const App: React.FC = () => {
  return (
    <>
      <Routes>
        <Route path="/" element={<Upload />} />
        <Route path="/settings/files" element={<FilesConfiguration />} />
        <Route path="/settings/emails" element={<EmailRegistration />} />
        <Route path="/settings/samples" element={<SamplesManagement />} />
        <Route path="/settings/batch-ingest" element={<BatchIngest />} />
      </Routes>
      <ToastContainer
        position="top-right"
        autoClose={3000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="dark"
      />
    </>
  );
};

export default App;
