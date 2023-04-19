using ImpactSpace.Core.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ImpactSpace.Core.Organizations;

public static class OrganizationsConfigurationExtensions
{
    public static void ConfigureOrganizations(this ModelBuilder builder)
    {
        builder.Entity<Organization>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "Organizations", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(OrganizationConstants.MaxNameLength);
            b.Property(x => x.Description)
                .HasMaxLength(OrganizationConstants.MaxDescriptionLength);
            b.HasOne(x => x.Tenant)
                .WithOne()
                .HasForeignKey<Organization>(x => x.TenantId)
                .IsRequired(false);
            
            b.HasOne(x => x.OrganizationProfile)
                .WithOne(x => x.Organization)
                .HasForeignKey<OrganizationProfile>(x => x.OrganizationId);
            
            b.HasIndex(o => o.TenantId).IsUnique();
        });
        
        builder.Entity<OrganizationProfile>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "OrganizationProfiles", CoreConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.MissionStatement)
                .HasMaxLength(OrganizationProfileConstants.MaxMissionStatementLength);
            b.Property(x => x.Website)
                .HasMaxLength(CommonConstants.MaxWebsiteLength);
            b.Property(x => x.Email)
                .HasMaxLength(CommonConstants.MaxEmailLength);
            b.Property(x => x.PhoneNumber)
                .HasMaxLength(CommonConstants.MaxNationalNumberLength);
            b.Property(x => x.LogoUrl)
                .HasMaxLength(OrganizationProfileConstants.MaxLogoUrlLength);
            
            b.OwnsMany(x => x.SocialMediaLinks, sm =>
            {
                sm.WithOwner().HasForeignKey("OrganizationProfileId");
                sm.ToTable(CoreConsts.DbTablePrefix + "OrganizationProfileSocialMediaLinks", CoreConsts.DbSchema);
                
                sm.Property(s => s.Url)
                    .HasMaxLength(CommonConstants.MaxWebsiteLength);
            });

            b.HasIndex(x => x.OrganizationId).IsUnique();
        });

        builder.Entity<OrganizationMember>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "OrganizationMembers", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(OrganizationMemberConsts.MaxNameLength);
            b.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(CommonConstants.MaxEmailLength);
            b.Property(x => x.PhoneNumber)
                .HasMaxLength(CommonConstants.MaxNationalNumberLength);

            b.HasOne(x => x.Organization)
                .WithMany(x => x.OrganizationMembers)
                .HasForeignKey(x => x.OrganizationId);
            b.HasOne(x => x.IdentityUser)
                .WithMany()
                .HasForeignKey(x => x.IdentityUserId)
                .IsRequired(false);
            b.HasMany(x => x.OrganizationMemberSkills)
                .WithOne(x => x.OrganizationMember)
                .HasForeignKey(x => x.OrganizationMemberId);
            b.HasMany(x => x.OrganizationMemberActions)
                .WithOne(x => x.OrganizationMember)
                .HasForeignKey(x => x.OrganizationMemberId);
            b.HasMany(x => x.OrganizationMemberChallenges)
                .WithOne(x => x.OrganizationMember)
                .HasForeignKey(x => x.OrganizationMemberId);
            b.HasMany(x => x.OrganizationMemberProjects)
                .WithOne(x => x.OrganizationMember)
                .HasForeignKey(x => x.OrganizationMemberId);
            b.HasMany(x => x.OwnedProjects)
                .WithOne(x => x.Owner)
                .HasForeignKey(x => x.OwnerId)
                .IsRequired(false);
            
            b.OwnsMany(x => x.SocialMediaLinks, sm =>
            {
                sm.WithOwner().HasForeignKey("OrganizationMemberId");
                sm.ToTable(CoreConsts.DbTablePrefix + "OrganizationMemberSocialMediaLinks", CoreConsts.DbSchema);

                sm.Property(s => s.Url)
                    .HasMaxLength(CommonConstants.MaxWebsiteLength);
            });
        });

        builder.Entity<OrganizationMemberSkill>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "OrganizationMemberSkills", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            // Define a composite key for OrganizationMemberSkill
            b.HasKey(x => new { x.OrganizationMemberId, x.SkillId });
            
            b.HasOne(x => x.OrganizationMember)
                .WithMany(x => x.OrganizationMemberSkills)
                .HasForeignKey(x => x.OrganizationMemberId);
            b.HasOne(x => x.Skill)
                .WithMany(x => x.OrganizationMemberSkills)
                .HasForeignKey(x => x.SkillId);
        });
        
        builder.Entity<OrganizationMemberAction>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "OrganizationMemberActions", CoreConsts.DbSchema);
            b.ConfigureByConvention();

            b.HasKey(x => new { x.OrganizationMemberId, x.ActionId });

            b.HasOne(x => x.OrganizationMember)
                .WithMany(x => x.OrganizationMemberActions)
                .HasForeignKey(x => x.OrganizationMemberId);

            b.HasOne(x => x.Action)
                .WithMany(x => x.OrganizationMemberActions)
                .HasForeignKey(x => x.ActionId);
        });
        
        builder.Entity<OrganizationMemberChallenge>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "OrganizationMemberChallenges", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.HasKey(x => new { x.OrganizationMemberId, x.ChallengeId });

            b.HasOne(x => x.OrganizationMember)
                .WithMany(x => x.OrganizationMemberChallenges)
                .HasForeignKey(x => x.OrganizationMemberId);

            b.HasOne(x => x.Challenge)
                .WithMany(x => x.OrganizationMemberChallenges)
                .HasForeignKey(x => x.ChallengeId);
        });
        
        builder.Entity<OrganizationMemberProject>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "OrganizationMemberProjects", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.HasKey(x => new { x.OrganizationMemberId, x.ProjectId });

            b.HasOne(x => x.OrganizationMember)
                .WithMany(x => x.OrganizationMemberProjects)
                .HasForeignKey(x => x.OrganizationMemberId);

            b.HasOne(x => x.Project)
                .WithMany(x => x.OrganizationMemberProjects)
                .HasForeignKey(x => x.ProjectId);
        });
    }
}