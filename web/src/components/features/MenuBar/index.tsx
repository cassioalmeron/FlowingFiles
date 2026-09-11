import React from 'react';
import { Link } from 'react-router-dom';
import { FolderIcon, GearIcon, ImportIcon, LayersIcon, MailIcon } from '../../icons';
import './styles.css';

const MenuBar: React.FC = () => {
  return (
    <div className="menubar">
      <div className="menubar__brand">
        <FolderIcon size={20} color="var(--accent-2)" />
        <span className="menubar__title">FlowingFiles</span>
      </div>
      <div className="menubar__actions">
        <Link to="/settings/samples" className="menubar__btn" aria-label="Training Samples">
          <LayersIcon size={16} />
        </Link>
        <Link to="/settings/batch-ingest" className="menubar__btn" aria-label="Batch Ingest">
          <ImportIcon size={16} />
        </Link>
        <Link to="/settings/emails" className="menubar__btn" aria-label="Email Registration">
          <MailIcon size={16} />
        </Link>
        <Link to="/settings/files" className="menubar__btn" aria-label="Files Configuration">
          <GearIcon size={16} />
        </Link>
      </div>
    </div>
  );
};

export default MenuBar;
