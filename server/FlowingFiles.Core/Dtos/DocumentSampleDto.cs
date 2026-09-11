namespace FlowingFiles.Core.Dtos;

public class DocumentSampleDto : DtoBase
{
    public int DocumentOptionId { get; set; }
    public string SourceFileName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DocumentSampleDetailDto : DtoBase
{
    public int DocumentOptionId { get; set; }
    public string SourceFileName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ExtractedText { get; set; }
}

public record DocumentSampleGroupDto(int DocumentOptionId, string DocumentOptionDescription, int Count, List<DocumentSampleDto> Samples);
