using ImpactSpace.Core.Projects;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ImpactSpace.Core.EntityFrameworkCore;

public static class ProjectsConfigurationExtensions
{
    public static void ConfigureProjects(this ModelBuilder builder)
    {
        builder.Entity<MilestoneVoteAggregate>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "MilestoneVoteAggregates", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.HasOne(x => x.Milestone)
                .WithMany()
                .HasForeignKey(x => x.MilestoneId);
        });
        
        // Add configurations for Tag
        builder.Entity<Tag>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "Tags", CoreConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(TagConstants.MaxNameLength);
            
            b.HasMany(x => x.ProjectTags)
                .WithOne(x => x.Tag)
                .HasForeignKey(x => x.TagId);
        });

        // Add configurations for ProjectTag
        builder.Entity<ProjectTag>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "ProjectTags", CoreConsts.DbSchema);
            b.ConfigureByConvention();

            b.HasKey(x => new { x.ProjectId, x.TagId });

            b.HasOne(x => x.Project)
                .WithMany(x => x.ProjectTags)
                .HasForeignKey(x => x.ProjectId);

            b.HasOne(x => x.Tag)
                .WithMany(x => x.ProjectTags)
                .HasForeignKey(x => x.TagId);
        });

        builder.Entity<Project>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "Projects", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MaxNameLength);
            b.Property(x => x.Description)
                .HasMaxLength(ProjectConstants.MaxDescriptionLength);
            b.Property(x => x.Purpose)
                .HasMaxLength(ProjectConstants.MaxPurposeLength);
            
            b.HasOne(x => x.ProjectCategory)
                .WithMany(x => x.Projects)
                .HasForeignKey(x => x.ProjectCategoryId);
            b.HasOne(x => x.ProjectOwner)
                .WithMany(x=>x.Projects)
                .HasForeignKey(x => x.ProjectOwnerId);
            b.HasOne(x => x.Organization)
                .WithMany(x => x.Projects)
                .HasForeignKey(x => x.OrganizationId);
            b.HasMany(x => x.ProjectTags)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId);
            b.HasMany(x => x.ProjectChallenges)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId);
        });

        builder.Entity<Action>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "Actions", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ActionConstants.MaxNameLength);
            b.Property(x => x.Description)
                .HasMaxLength(ActionConstants.MaxDescriptionLength);
            
            b.HasOne(x => x.Objective)
                .WithMany(x => x.Actions)
                .HasForeignKey(x => x.ObjectiveId);
            b.HasMany(x => x.OrganizationMemberActions)
                .WithOne(x => x.Action)
                .HasForeignKey(x => x.ActionId);
        });
        
        builder.Entity<ProjectCategory>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "ProjectCategories", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ProjectCategoryConsts.MaxNameLength);

            b.HasMany(x => x.Projects)
                .WithOne(x => x.ProjectCategory)
                .HasForeignKey(x => x.ProjectCategoryId)
                .IsRequired();
        });

        builder.Entity<ProjectSkill>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "ProjectSkills", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.HasKey(x => new { x.ProjectId, x.SkillId });
            
            b.HasOne(x => x.Project)
                .WithMany(x => x.RequiredSkills)
                .HasForeignKey(x => x.ProjectId);
            b.HasOne(x => x.Skill)
                .WithMany(x => x.ProjectSkills)
                .HasForeignKey(x => x.SkillId);
        });

        builder.Entity<Objective>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "Objectives", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ObjectiveConstants.MaxNameLength);
            
            b.Property(x => x.Description)
                .HasMaxLength(ObjectiveConstants.MaxDescriptionLength);
            b.HasOne(x => x.Milestone)
                .WithMany(x => x.Objectives)
                .HasForeignKey(x => x.MilestoneId);
            
        });
        
        builder.Entity<Milestone>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "Milestones", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(MilestoneConstants.MaxNameLength);
            b.Property(x => x.Description)
                .HasMaxLength(MilestoneConstants.MaxDescriptionLength);
            
            b.HasOne(x => x.Project)
                .WithMany(x => x.Milestones)
                .HasForeignKey(x => x.ProjectId);
        });

        builder.Entity<MilestoneVote>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "MilestoneVotes", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.HasKey(x => new { x.MilestoneId, x.OrganizationMemberId });
            
            b.HasOne(x => x.OrganizationMember)
                .WithMany()
                .HasForeignKey(x => x.OrganizationMemberId);
            b.HasOne(x => x.MilestoneVoteAggregate)
                .WithMany(x => x.Votes)
                .HasForeignKey(x => x.MilestoneVoteAggregateId);
        });
    }
}