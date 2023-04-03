using ImpactSpace.Core.Organizations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ImpactSpace.Core.EntityFrameworkCore;

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
            
            b.HasIndex(o => o.TenantId).IsUnique();
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
                .HasMaxLength(OrganizationMemberConsts.MaxEmailLength);
            b.Property(x => x.Phone)
                .HasMaxLength(OrganizationMemberConsts.MaxPhoneLength);

            b.HasOne(x => x.Organization)
                .WithMany(x => x.OrganizationMembers) // Add this line
                .HasForeignKey(x => x.OrganizationId);
            b.HasMany(x => x.Projects)
                .WithOne(x => x.ProjectOwner)
                .HasForeignKey(x => x.ProjectOwnerId);
            b.HasMany(x => x.Skills)
                .WithOne(x => x.OrganizationMember)
                .HasForeignKey(x => x.OrganizationMemberId);
            b.HasMany(x => x.OrganizationMemberActions)
                .WithOne(x => x.OrganizationMember)
                .HasForeignKey(x => x.OrganizationMemberId);
            b.HasMany(x => x.OrganizationMemberChallenges)
                .WithOne(x => x.OrganizationMember)
                .HasForeignKey(x => x.OrganizationMemberId);
        });

        builder.Entity<OrganizationMemberSkill>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "OrganizationMemberSkills", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            // Define a composite key for OrganizationMemberSkill
            b.HasKey(x => new { x.OrganizationMemberId, x.SkillId });
            
            b.HasOne(x => x.OrganizationMember)
                .WithMany(x => x.Skills)
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
    }
}