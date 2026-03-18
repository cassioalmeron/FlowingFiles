namespace FlowingFiles.Core.Dtos;

public class EmailDestinationDto : DtoBase
{
    public string EmailAddress { get; set; }
    public bool Active { get; set; }
}
