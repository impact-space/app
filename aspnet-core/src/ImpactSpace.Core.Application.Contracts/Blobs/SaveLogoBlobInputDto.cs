using System.ComponentModel.DataAnnotations;

namespace ImpactSpace.Core.Blobs;

public class SaveLogoBlobInputDto
{
    public byte[] Content { get; set; }
    
    [Required] 
    public string Name { get; set; }
}