namespace FlowingFiles.Core.Dtos;

public class DocumentOptionDto : DtoBase
{
    public string Description { get; set; }
    public string Path { get; set; }
    public bool Required { get; set; }
    public int Position { get; set; }
}
