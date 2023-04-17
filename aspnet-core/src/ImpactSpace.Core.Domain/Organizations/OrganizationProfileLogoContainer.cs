using System.Linq;
using System.Reflection;
using Volo.Abp.BlobStoring;

namespace ImpactSpace.Core.Organizations;

[BlobContainerName("organization-profile-logo-container")]
public class OrganizationProfileLogoContainer
{
    public static string GetContainerName()
    {
        var containerType = typeof(OrganizationProfileLogoContainer);
        var containerNameAttribute = containerType.GetCustomAttributes<BlobContainerNameAttribute>().FirstOrDefault();
        return containerNameAttribute?.Name;
    }
}