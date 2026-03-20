import React, { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import { getEmailDestinations, sendEmail as sendEmailApi } from '../../../lib/api';
import { generateZip } from '../../../lib/zip';
import { SendIcon, TrashIcon, PlusIcon, MailIcon } from '../../icons';
import type { FileEntry, EmailDestinationItem } from '../../../types';
import './styles.css';

const MONTHS_PT = [
  'JANEIRO', 'FEVEREIRO', 'MARÇO', 'ABRIL', 'MAIO', 'JUNHO',
  'JULHO', 'AGOSTO', 'SETEMBRO', 'OUTUBRO', 'NOVEMBRO', 'DEZEMBRO',
];

interface SendEmailProps {
  isOpen: boolean;
  onClose: () => void;
  selectedMonth: number;
  selectedYear: number;
  files: FileEntry[];
  monthAbbrev: string;
}

const SendEmail: React.FC<SendEmailProps> = ({
  isOpen,
  onClose,
  selectedMonth,
  selectedYear,
  files,
  monthAbbrev,
}) => {
  const [destinations, setDestinations] = useState<EmailDestinationItem[]>([]);
  const [subject, setSubject] = useState('');
  const [body, setBody] = useState('');
  const [sending, setSending] = useState(false);
  const [extraFiles, setExtraFiles] = useState<File[]>([]);
  const [loading, setLoading] = useState(false);

  // Generate default subject and body from templates
  const generateDefaults = () => {
    const monthPt = MONTHS_PT[selectedMonth - 1];
    const defaultSubject = `CASSIO ALMERON SOFTWARE ENGINEERING LTDA - Documentos Mensais - ${monthPt} ${selectedYear}`;
    const defaultBody = `Boa tarde\n\nSegue os documentos referentes ao mês de ${monthPt} de ${selectedYear}.\n\nQualquer dúvida estou à disposição.\nIclen Granzotto`;
    return { defaultSubject, defaultBody };
  };

  // Fetch email destinations on open
  useEffect(() => {
    if (!isOpen) return;

    const fetchDestinations = async () => {
      try {
        setLoading(true);
        const data = await getEmailDestinations();
        const activeDestinations = data.filter((d) => d.active);
        setDestinations(activeDestinations);
        setExtraFiles([]);

        const { defaultSubject, defaultBody } = generateDefaults();
        setSubject(defaultSubject);
        setBody(defaultBody);
      } catch (error) {
        toast.error('Failed to load email destinations');
        console.error('Error fetching email destinations:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchDestinations();
  }, [isOpen, selectedMonth, selectedYear]);

  const handleAddFiles = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = Array.from(e.target.files || []);
    setExtraFiles((prev) => [...prev, ...files]);
    // Reset input
    e.target.value = '';
  };

  const handleRemoveExtraFile = (index: number) => {
    setExtraFiles((prev) => prev.filter((_, i) => i !== index));
  };

  const handleSend = async () => {
    if (destinations.length === 0) {
      toast.warning('No active email recipients found');
      return;
    }

    const filledFiles = files.filter((f) => f.file !== null);
    if (filledFiles.length === 0) {
      toast.warning('No files to send');
      return;
    }

    setSending(true);
    const toastId = toast.loading('Sending email to all recipients...');

    try {
      // Generate ZIP blob
      const zipBlob = await generateZip(files, monthAbbrev);

      // Build attachments array: ZIP first, then extra files
      const attachments = [
        { blob: zipBlob, name: `${monthAbbrev}.zip` },
        ...extraFiles.map((f) => ({ blob: f, name: f.name })),
      ];

      // Get recipient emails as comma-separated string
      const recipients = destinations.map((d) => d.emailAddress).join(',');

      // Send via API
      await sendEmailApi(recipients, subject, body, attachments);

      toast.update(toastId, {
        render: 'Email sent successfully to all recipients!',
        type: 'success',
        isLoading: false,
        autoClose: 3000,
      });

      // Close modal on success
      onClose();
    } catch (error) {
      console.error('Error sending email:', error);
      toast.update(toastId, {
        render: 'Failed to send email. Please try again.',
        type: 'error',
        isLoading: false,
        autoClose: 3000,
      });
    } finally {
      setSending(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="send-email-backdrop" onClick={onClose}>
      <div className="send-email-dialog" onClick={(e) => e.stopPropagation()}>
        {/* Header */}
        <div className="send-email-header">
          <div className="send-email-title">
            <MailIcon size={20} />
            <h2>Send Email</h2>
          </div>
          <button
            onClick={onClose}
            className="send-email-close"
            aria-label="Close"
            disabled={sending}
          >
            ✕
          </button>
        </div>

        {/* Content */}
        <div className="send-email-content">
          {loading ? (
            <div className="send-email-loading">Loading email addresses...</div>
          ) : (
            <>
              {/* Recipients Display */}
              <div className="send-email-field">
                <label>Recipients ({destinations.length})</label>
                <div className="send-email-recipients">
                  {destinations.length > 0 ? (
                    destinations.map((dest) => (
                      <div key={dest.id} className="send-email-recipient-item">
                        {dest.emailAddress}
                      </div>
                    ))
                  ) : (
                    <div className="send-email-no-recipients">No active recipients</div>
                  )}
                </div>
              </div>

              {/* Subject */}
              <div className="send-email-field">
                <label htmlFor="send-email-subject">Subject</label>
                <input
                  id="send-email-subject"
                  type="text"
                  value={subject}
                  onChange={(e) => setSubject(e.target.value)}
                  disabled={sending}
                  className="send-email-input"
                />
              </div>

              {/* Body */}
              <div className="send-email-field">
                <label htmlFor="send-email-body">Body</label>
                <textarea
                  id="send-email-body"
                  value={body}
                  onChange={(e) => setBody(e.target.value)}
                  disabled={sending}
                  className="send-email-textarea"
                  rows={5}
                />
              </div>

              {/* Attachments */}
              <div className="send-email-field">
                <label>Attachments</label>
                <div className="send-email-attachments">
                  {/* ZIP attachment (always present, not removable) */}
                  <div className="send-email-attachment-item send-email-attachment-zip">
                    <span>{monthAbbrev}.zip</span>
                    <span className="send-email-attachment-badge">ZIP</span>
                  </div>

                  {/* Extra files */}
                  {extraFiles.map((file, index) => (
                    <div key={index} className="send-email-attachment-item">
                      <span>{file.name}</span>
                      <button
                        onClick={() => handleRemoveExtraFile(index)}
                        disabled={sending}
                        className="send-email-attachment-remove"
                        aria-label="Remove file"
                      >
                        <TrashIcon size={16} />
                      </button>
                    </div>
                  ))}

                  {/* Add files button */}
                  <label className="send-email-add-files">
                    <PlusIcon size={16} />
                    <span>Add files</span>
                    <input
                      type="file"
                      multiple
                      onChange={handleAddFiles}
                      disabled={sending}
                      style={{ display: 'none' }}
                    />
                  </label>
                </div>
              </div>
            </>
          )}
        </div>

        {/* Footer */}
        <div className="send-email-footer">
          <button
            onClick={onClose}
            disabled={sending}
            className="send-email-btn send-email-btn-cancel"
          >
            Cancel
          </button>
          <button
            onClick={handleSend}
            disabled={sending || destinations.length === 0 || loading}
            className="send-email-btn send-email-btn-send"
          >
            {sending ? (
              <>
                <span className="send-email-spinner"></span>
                Sending...
              </>
            ) : (
              <>
                <SendIcon size={16} />
                Send
              </>
            )}
          </button>
        </div>
      </div>
    </div>
  );
};

export default SendEmail;
