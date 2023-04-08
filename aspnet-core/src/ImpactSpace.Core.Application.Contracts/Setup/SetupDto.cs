using ImpactSpace.Core.Organizations;
using System.ComponentModel.DataAnnotations;

namespace ImpactSpace.Core.Setup
{
    public class SetupDto
    {
        [Required]
        [StringLength(OrganizationConstants.MaxNameLength)]
        public string OrganizationName { get; set; }
        
        [StringLength(OrganizationConstants.MaxDescriptionLength)]
        public string OrganizationDescription { get; set; }
        
        [Required]
        [StringLength(OrganizationMemberConsts.MaxNameLength)]
        public string MemberName { get; set; }
        
        [Required]
        [StringLength(OrganizationMemberConsts.MaxEmailLength)]
        public string MemberEmail { get; set; }
    }
}