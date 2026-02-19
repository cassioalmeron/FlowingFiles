import React from 'react';
import { Link } from 'react-router-dom';
import { FolderIcon, GearIcon } from '../../icons';
import './styles.css';

interface MenuBarProps {
  onExportZip?: () => void;
}

const MenuBar: React.FC<MenuBarProps> = ({ onExportZip }) => {
  return (
    <div className="menubar">
      <div className="menubar__brand">
        <FolderIcon size={20} color="var(--accent-2)" />
        <span className="menubar__title">FlowingFiles</span>
      </div>
      <div className="menubar__actions">
        {onExportZip && (
          <button className="menubar__btn" onClick={onExportZip}>
            Export ZIP
          </button>
        )}
        <Link to="/settings/files" className="menubar__btn" aria-label="Files Configuration">
          <GearIcon size={16} />
        </Link>
      </div>
    </div>
  );
};

export default MenuBar;
