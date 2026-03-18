import React, { useState, useCallback, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import type { EmailDestinationItem } from '../../types';
import { API_URL } from '../../lib/api';
import { ArrowLeftIcon, PlusIcon, TrashIcon } from '../../components/icons';
import './styles.css';

const EmailRegistration: React.FC = () => {
  const navigate = useNavigate();
  const [destinations, setDestinations] = useState<EmailDestinationItem[]>([]);
  const [savedSnapshot, setSavedSnapshot] = useState<string>('[]');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch(`${API_URL}/emaildestination`)
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json() as Promise<EmailDestinationItem[]>;
      })
      .then((data) => {
        setDestinations(data);
        setSavedSnapshot(JSON.stringify(data));
      })
      .catch(() => {
        toast.error('Failed to load email destinations');
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  const hasChanges = JSON.stringify(destinations) !== savedSnapshot;

  const addDestination = useCallback(() => {
    setDestinations((prev) => [
      ...prev,
      { id: Date.now() * -1, emailAddress: '', active: true },
    ]);
  }, []);

  const updateDestination = useCallback(
    (id: number, field: keyof EmailDestinationItem, value: string | boolean) => {
      setDestinations((prev) =>
        prev.map((d) => (d.id === id ? { ...d, [field]: value } : d))
      );
    },
    []
  );

  const removeDestination = useCallback(
    (id: number) => {
      const dest = destinations.find((d) => d.id === id);
      const label = dest?.emailAddress || 'this entry';

      toast(
        ({ closeToast }) => (
          <div>
            <p>Remove "{label}"?</p>
            <div style={{ display: 'flex', gap: '8px', marginTop: '8px' }}>
              <button
                onClick={() => {
                  setDestinations((prev) => prev.filter((d) => d.id !== id));
                  closeToast?.();
                  toast.success('Entry removed');
                }}
                style={{
                  padding: '4px 12px',
                  background: 'var(--status-required)',
                  color: '#fff',
                  border: 'none',
                  borderRadius: '4px',
                  cursor: 'pointer',
                }}
              >
                Remove
              </button>
              <button
                onClick={() => closeToast?.()}
                style={{
                  padding: '4px 12px',
                  background: 'var(--bg-6)',
                  color: 'var(--fg-primary)',
                  border: 'none',
                  borderRadius: '4px',
                  cursor: 'pointer',
                }}
              >
                Cancel
              </button>
            </div>
          </div>
        ),
        { autoClose: false, closeOnClick: false }
      );
    },
    [destinations]
  );

  const handleSave = useCallback(async () => {
    const emptyAddresses = destinations.filter((d) => !d.emailAddress.trim());
    if (emptyAddresses.length > 0) {
      toast.error('All entries must have an email address');
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const invalidAddresses = destinations.filter(
      (d) => !emailRegex.test(d.emailAddress.trim())
    );
    if (invalidAddresses.length > 0) {
      toast.error(`Invalid email address: "${invalidAddresses[0].emailAddress}"`);
      return;
    }

    const addresses = destinations.map((d) => d.emailAddress.trim().toLowerCase());
    const duplicates = addresses.filter((a, i) => addresses.indexOf(a) !== i);
    if (duplicates.length > 0) {
      toast.error(`Duplicate email address: "${duplicates[0]}"`);
      return;
    }

    try {
      const res = await fetch(`${API_URL}/emaildestination`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(
          destinations.map((d) => ({ ...d, id: d.id < 0 ? 0 : d.id }))
        ),
      });

      if (!res.ok) throw new Error(`HTTP ${res.status}`);

      const saved = (await res.json()) as EmailDestinationItem[];
      setDestinations(saved);
      setSavedSnapshot(JSON.stringify(saved));
      toast.success('Email destinations saved');
    } catch {
      toast.error('Failed to save email destinations');
    }
  }, [destinations]);

  const handleBack = useCallback(() => {
    if (hasChanges) {
      toast(
        ({ closeToast }) => (
          <div>
            <p>You have unsaved changes. Discard?</p>
            <div style={{ display: 'flex', gap: '8px', marginTop: '8px' }}>
              <button
                onClick={() => {
                  closeToast?.();
                  navigate('/');
                }}
                style={{
                  padding: '4px 12px',
                  background: 'var(--status-required)',
                  color: '#fff',
                  border: 'none',
                  borderRadius: '4px',
                  cursor: 'pointer',
                }}
              >
                Discard
              </button>
              <button
                onClick={() => closeToast?.()}
                style={{
                  padding: '4px 12px',
                  background: 'var(--bg-6)',
                  color: 'var(--fg-primary)',
                  border: 'none',
                  borderRadius: '4px',
                  cursor: 'pointer',
                }}
              >
                Stay
              </button>
            </div>
          </div>
        ),
        { autoClose: false, closeOnClick: false }
      );
    } else {
      navigate('/');
    }
  }, [hasChanges, navigate]);

  return (
    <div className="email-reg">
      <div className="email-reg__header">
        <button className="email-reg__back" onClick={handleBack} aria-label="Back">
          <ArrowLeftIcon size={20} />
        </button>
        <h1 className="email-reg__title">Email Registration</h1>
        {hasChanges && <span className="email-reg__unsaved">Unsaved changes</span>}
      </div>

      <div className="email-reg__content">
        {loading ? (
          <div className="email-reg__loading">Loading...</div>
        ) : (
          <div className="email-reg__table-wrapper">
            <table className="email-reg__table">
              <thead>
                <tr>
                  <th className="email-reg__th email-reg__th--email">Email Address</th>
                  <th className="email-reg__th email-reg__th--active">Active</th>
                  <th className="email-reg__th email-reg__th--actions"></th>
                </tr>
              </thead>
              <tbody>
                {destinations.map((dest) => (
                  <tr key={dest.id} className="email-reg__row">
                    <td className="email-reg__td email-reg__td--email">
                      <input
                        type="email"
                        className="email-reg__input"
                        value={dest.emailAddress}
                        onChange={(e) =>
                          updateDestination(dest.id, 'emailAddress', e.target.value)
                        }
                        placeholder="name@example.com"
                      />
                    </td>
                    <td className="email-reg__td email-reg__td--active">
                      <input
                        type="checkbox"
                        className="email-reg__checkbox"
                        checked={dest.active}
                        onChange={(e) =>
                          updateDestination(dest.id, 'active', e.target.checked)
                        }
                      />
                    </td>
                    <td className="email-reg__td email-reg__td--actions">
                      <button
                        className="email-reg__delete-btn"
                        onClick={() => removeDestination(dest.id)}
                        aria-label={`Remove ${dest.emailAddress}`}
                      >
                        <TrashIcon size={16} color="var(--status-required)" />
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>

            {destinations.length === 0 && (
              <div className="email-reg__empty">
                No email destinations configured. Click "Add Email" to get started.
              </div>
            )}
          </div>
        )}
      </div>

      <div className="email-reg__actions">
        <button className="email-reg__add-btn" onClick={addDestination} disabled={loading}>
          <PlusIcon size={16} />
          Add Email
        </button>
        <button
          className="email-reg__save-btn"
          onClick={handleSave}
          disabled={!hasChanges || loading}
        >
          Save
        </button>
      </div>
    </div>
  );
};

export default EmailRegistration;
