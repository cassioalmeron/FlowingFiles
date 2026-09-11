namespace FlowingFiles.Core;

public class OllamaSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string EmbeddingModel { get; set; } = string.Empty;
    public double Threshold { get; set; }
    public int K { get; set; }
}
