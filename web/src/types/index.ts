export const FileStatus = {
  EmptyRequired: 'EmptyRequired',
  EmptyOptional: 'EmptyOptional',
  Filled: 'Filled',
} as const;

export type FileStatus = (typeof FileStatus)[keyof typeof FileStatus];

export interface DocumentOption {
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

export interface FileEntry {
  option: DocumentOption;
  file: File | null;
}

export function getFileStatus(entry: FileEntry): FileStatus {
  if (entry.file) return FileStatus.Filled;
  if (entry.option.required) return FileStatus.EmptyRequired;
  return FileStatus.EmptyOptional;
}
