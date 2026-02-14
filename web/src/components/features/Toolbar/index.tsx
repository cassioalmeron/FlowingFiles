import React from 'react';
import { DownloadIcon } from '../../icons';
import './styles.css';

const MONTHS = [
  'January', 'February', 'March', 'April', 'May', 'June',
  'July', 'August', 'September', 'October', 'November', 'December',
];

interface ToolbarProps {
  selectedMonth: number;
  onMonthChange: (month: number) => void;
  onExportZip: () => void;
  filledCount: number;
  totalCount: number;
}

const Toolbar: React.FC<ToolbarProps> = ({
  selectedMonth,
  onMonthChange,
  onExportZip,
  filledCount,
  totalCount,
}) => {
  return (
    <div className="toolbar">
      <div className="toolbar__left">
        <label className="toolbar__label" htmlFor="month-select">
          Month:
        </label>
        <select
          id="month-select"
          className="toolbar__select"
          value={selectedMonth}
          onChange={(e) => onMonthChange(Number(e.target.value))}
        >
          {MONTHS.map((name, i) => (
            <option key={i + 1} value={i + 1}>
              {name}
            </option>
          ))}
        </select>
      </div>

      <div className="toolbar__right">
        <span className="toolbar__status">
          {filledCount} / {totalCount} files
        </span>
        <button
          className="toolbar__export-btn"
          onClick={onExportZip}
          disabled={filledCount === 0}
        >
          <DownloadIcon size={16} />
          Export as ZIP
        </button>
      </div>
    </div>
  );
};

export default Toolbar;
