import JSZip from 'jszip';
import type { FileEntry } from '../types';

export async function generateZip(files: FileEntry[], monthAbbrev: string): Promise<Blob> {
  const filledEntries = files.filter((e) => e.file !== null);

  if (filledEntries.length === 0) {
    throw new Error('No files to include in ZIP');
  }

  const zip = new JSZip();

  for (const entry of filledEntries) {
    const ext = entry.file!.name.includes('.')
      ? '.' + entry.file!.name.split('.').pop()
      : '';
    const zipPath = `${entry.option.path}${ext}`;
    const buffer = await entry.file!.arrayBuffer();
    zip.file(zipPath, buffer);
  }

  return zip.generateAsync({ type: 'blob', comment: monthAbbrev });
}
