import JSZip from 'jszip';
import type { FileEntry } from '../types';

const METADATA_ENTRIES = ['.DS_Store', 'Thumbs.db'];

const CONTENT_TYPES: Record<string, string> = {
  pdf: 'application/pdf',
  png: 'image/png',
  jpg: 'image/jpeg',
  jpeg: 'image/jpeg',
  gif: 'image/gif',
  bmp: 'image/bmp',
  webp: 'image/webp',
  svg: 'image/svg+xml',
  xml: 'application/xml',
  html: 'text/html',
  json: 'application/json',
  csv: 'text/csv',
  txt: 'text/plain',
  ofx: 'text/plain',
};

export interface ZipImportEntry {
  /** Full entry path inside the archive, with separators normalised. */
  path: string;
  /** `path` without its extension — the key that matches `DocumentOption.path`. */
  key: string;
  file: File;
}

export interface ZipImportResult {
  entries: ZipImportEntry[];
  /** Three-letter month abbreviation read from the archive comment, when present. */
  monthAbbrev: string | null;
}

export class InvalidZipError extends Error {
  constructor() {
    super('The selected file could not be read as a ZIP archive.');
    this.name = 'InvalidZipError';
  }
}

function normalizePath(value: string): string {
  return value.replace(/\\/g, '/').replace(/^\/+/, '');
}

function extensionOf(path: string): string {
  const lastSlash = path.lastIndexOf('/');
  const lastDot = path.lastIndexOf('.');
  return lastDot > lastSlash + 1 ? path.substring(lastDot + 1).toLowerCase() : '';
}

function stripExtension(path: string): string {
  const extension = extensionOf(path);
  return extension ? path.substring(0, path.length - extension.length - 1) : path;
}

function basenameOf(path: string): string {
  return path.substring(path.lastIndexOf('/') + 1);
}

function isMetadata(path: string): boolean {
  return path.startsWith('__MACOSX/') || METADATA_ENTRIES.includes(basenameOf(path));
}

/** Normalises a `DocumentOption.path` so it can be compared against `ZipImportEntry.key`. */
export function documentPathKey(path: string): string {
  return normalizePath(path);
}

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

export async function parseZip(archive: File): Promise<ZipImportResult> {
  let zip: JSZip;
  try {
    // Read the bytes first: jszip's Blob path goes through FileReader, which the
    // ArrayBuffer path avoids entirely.
    zip = await JSZip.loadAsync(await archive.arrayBuffer());
  } catch {
    throw new InvalidZipError();
  }

  const entries: ZipImportEntry[] = [];

  for (const zipEntry of Object.values(zip.files)) {
    if (zipEntry.dir)
      continue;

    const path = normalizePath(zipEntry.name);
    if (isMetadata(path))
      continue;

    const blob = await zipEntry.async('blob');
    const contentType = CONTENT_TYPES[extensionOf(path)] ?? 'application/octet-stream';
    const file = new File([blob], basenameOf(path), { type: contentType });

    entries.push({ path, key: stripExtension(path), file });
  }

  // `loadAsync` populates the archive comment at runtime, but jszip's typings omit it.
  const comment = (zip as JSZip & { comment?: string }).comment;

  return { entries, monthAbbrev: comment || null };
}
