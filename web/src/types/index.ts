export const FileStatus = {
  EmptyRequired: 'EmptyRequired',
  EmptyOptional: 'EmptyOptional',
  Filled: 'Filled',
} as const;

export type FileStatus = (typeof FileStatus)[keyof typeof FileStatus];

export interface DocumentOption {
  id: number;
  description: string;
  path: string;
  required: boolean;
}

export interface DocumentOptionItem {
  id: number;
  description: string;
  path: string;
  required: boolean;
  position: number;
}

export interface SendEmailRequest {
  to: string;
  subject: string;
  body: string;
  // attachments: ZIP (always) + optional extra files — sent as FormData
}

export interface EmailDestinationItem {
  id: number;
  emailAddress: string;
  active: boolean;
}

export interface DocumentSampleItem {
  id: number;
  documentOptionId: number;
  sourceFileName: string;
  createdAt: string;
}

export interface DocumentSampleGroup {
  documentOptionId: number;
  documentOptionDescription: string;
  count: number;
  samples: DocumentSampleItem[];
}

export interface DocumentSampleDetail extends DocumentSampleItem {
  extractedText: string;
}

export interface NeighbourScore {
  label: string;
  similarity: number;
}

export interface FileClassificationResult {
  label: string;
  method: 'Rule' | 'Similarity';
  neighbours: NeighbourScore[] | null;
}

export interface FileEntry {
  option: DocumentOption;
  file: File | null;
  /** Set by auto-classify; how the classifier picked this file's slot, and its nearest-neighbour ranking when applicable. */
  classification?: FileClassificationResult;
}

export function getFileStatus(entry: FileEntry): FileStatus {
  if (entry.file) return FileStatus.Filled;
  if (entry.option.required) return FileStatus.EmptyRequired;
  return FileStatus.EmptyOptional;
}
