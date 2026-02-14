import React from 'react';
import { FolderIcon } from '../../icons';
import './styles.css';

interface MenuBarProps {
  onExportZip: () => void;
}

const MenuBar: React.FC<MenuBarProps> = ({ onExportZip }) => {
  return (
    <div className="menubar">
      <div className="menubar__brand">
        <FolderIcon size={20} color="var(--accent-2)" />
        <span className="menubar__title">FlowingFiles</span>
      </div>
      <div className="menubar__actions">
        <button className="menubar__btn" onClick={onExportZip}>
          Export ZIP
        </button>
      </div>
    </div>
  );
};

export default MenuBar;
