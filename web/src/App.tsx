import React from 'react';
import { Routes, Route } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Upload from './pages/Upload';
import FilesConfiguration from './pages/FilesConfiguration';

const App: React.FC = () => {
  return (
    <>
      <Routes>
        <Route path="/" element={<Upload />} />
        <Route path="/settings/files" element={<FilesConfiguration />} />
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
