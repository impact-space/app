using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImpactSpace.Core.Common;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Organizations;

public class OrganizationProfile : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid OrganizationId { get; private set; }
    
    public virtual Organization Organization { get; private set; }
    
    public string MissionStatement { get; private set; }
    
    public string Website { get; private set; }
    
    public string PhoneNumber { get; private set; }
    
    public string Email { get; private set; }
    
    public string LogoUrl { get; private set; }
    
    public virtual ICollection<SocialMediaLink> SocialMediaLinks { get; private set; }
    
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public Guid? TenantId { get; set; }

    protected OrganizationProfile()
    {
    }

    internal OrganizationProfile(
        Guid id, 
        Guid organizationId, 
        [CanBeNull] string missionStatement, 
        [CanBeNull] string websiteUrl,
        [CanBeNull] string phoneNumber,
        [CanBeNull] string email,
        [CanBeNull] string logoUrl)
        : base(id)
    {
        if (organizationId == Guid.Empty)
        {
            throw new ArgumentException("Organization Id cannot be empty", nameof(organizationId));
        }
        
        OrganizationId = organizationId;
        SetMissionStatement(missionStatement);
        SetWebsite(websiteUrl);
        SetLogoUrl(logoUrl);
        SetPhoneNumber(phoneNumber);
        SetEmail(email);
        SocialMediaLinks = new Collection<SocialMediaLink>();
    }

    internal OrganizationProfile ChangeMissionStatement([CanBeNull] string missionStatement)
    {
        SetMissionStatement(missionStatement);
        return this;
    }

    internal OrganizationProfile ChangeWebsite([CanBeNull] string websiteUrl)
    {
        SetWebsite(websiteUrl);
        return this;
    }
    
    internal OrganizationProfile ChangePhoneNumber([CanBeNull] string phoneNumber)
    {
        SetPhoneNumber(phoneNumber);
        return this;
    }

    
    internal OrganizationProfile ChangeEmail([CanBeNull] string email)
    {
        SetEmail(email);
        return this;
    }

    internal OrganizationProfile ChangeLogoUrl([CanBeNull] string logoUrl)
    {
        SetLogoUrl(logoUrl);
        return this;
    }

    private void SetMissionStatement([CanBeNull] string missionStatement)
    {
        MissionStatement = missionStatement;
    }

    private void SetWebsite([CanBeNull] string websiteUrl)
    {
        if (string.IsNullOrWhiteSpace(websiteUrl))
        {
            Website = null;
            return;
        }

        if (!ValidationHelper.IsValidUrl(websiteUrl))
        {
            throw new ArgumentException("The provided website URL is not valid.", nameof(websiteUrl));
        }

        Website = websiteUrl;
    }
    
    private void SetPhoneNumber([CanBeNull] string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
    
    private void SetEmail([CanBeNull] string email)
    {
        if (!email.IsNullOrWhiteSpace() && !ValidationHelper.IsValidEmail(email))
        {
            throw new ArgumentException("The provided email is not valid.", nameof(email));
        }

        Email = email;
    }

    private void SetLogoUrl([CanBeNull] string logoUrl)
    {
        LogoUrl = logoUrl;
    }
    
    internal void AddSocialMediaLink(SocialMediaLink socialMediaLink)
    {
        SocialMediaLinks.Add(socialMediaLink);
    }

    internal void RemoveSocialMediaLink(SocialMediaLink socialMediaLink)
    {
        SocialMediaLinks.Remove(socialMediaLink);
    }
}