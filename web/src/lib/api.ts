import type { EmailDestinationItem } from '../types';

export const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5000';

export async function getEmailDestinations(): Promise<EmailDestinationItem[]> {
  const res = await fetch(`${API_URL}/emaildestination`);
  if (!res.ok) throw new Error(`HTTP ${res.status}`);
  return res.json();
}

export async function sendEmail(
  to: string,
  subject: string,
  body: string,
  attachments: { blob: Blob; name: string }[]
): Promise<void> {
  const form = new FormData();
  form.append('to', to);
  form.append('subject', subject);
  form.append('body', body);
  for (const { blob, name } of attachments) {
    form.append('attachments', blob, name);
  }

  const res = await fetch(`${API_URL}/email/send`, { method: 'POST', body: form });
  if (!res.ok) throw new Error(`HTTP ${res.status}`);
}
