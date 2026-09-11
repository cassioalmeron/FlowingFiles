namespace FlowingFiles.Core.Dtos;

public record ZipImportFailureDto(string Path, string Reason);

public record ImportZipResultDto(int Requested, int Ingested, List<ZipImportFailureDto> Failed);
