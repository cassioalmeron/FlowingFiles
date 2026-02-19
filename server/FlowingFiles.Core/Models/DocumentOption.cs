namespace FlowingFiles.Core.Models
{
    public record DocumentOption : EntityBase
    {
        public string Description { get; set; }
        public string Path { get; set; }
        public bool Required { get; set; }
        public int Position { get; set; }
    }
}
