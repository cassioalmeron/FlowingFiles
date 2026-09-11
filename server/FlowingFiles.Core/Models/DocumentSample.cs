namespace FlowingFiles.Core.Models;

public record DocumentSample : EntityBase
{
    public int DocumentOptionId { get; set; }
    public DocumentOption DocumentOption { get; set; } = null!;
    public string SourceFileName { get; set; }
    public string ExtractedText { get; set; }
    public float[] Embedding { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
