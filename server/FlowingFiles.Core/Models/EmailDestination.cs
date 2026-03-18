namespace FlowingFiles.Core.Models
{
    public record EmailDestination : EntityBase
    {
        public string EmailAddress { get; set; }
        public bool Active { get; set; }
    }
}
