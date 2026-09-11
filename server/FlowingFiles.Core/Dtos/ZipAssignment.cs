namespace FlowingFiles.Core.Dtos;

/// <summary>
/// One (documentOptionId, path) pairing decided by the client's own zip-matching logic
/// (web/src/lib/zip.ts) — the server only extracts and ingests the entries it is told about, it
/// never re-derives the path-to-slot mapping itself.
/// </summary>
public record ZipAssignment(int DocumentOptionId, string Path);
