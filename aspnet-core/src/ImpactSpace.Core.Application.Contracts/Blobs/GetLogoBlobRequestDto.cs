using System.ComponentModel.DataAnnotations;

namespace ImpactSpace.Core.Blobs;

public class GetLogoBlobRequestDto
{
    [Required]
    public string Name { get; set; } 
}