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
                .HasMaxLength(32);
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
                .WithMany()
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
        });

        builder.Entity<Objective>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "Activities", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ActivityConstants.MaxNameLength);
            b.Property(x => x.Description)
                .HasMaxLength(ActivityConstants.MaxDescriptionLength);
            
            b.HasOne(x => x.Quest)
                .WithMany(x => x.Objectives)
                .HasForeignKey(x => x.QuestId);
            b.HasMany(x => x.OrganizationMemberActivities)
                .WithOne(x => x.Objective)
                .HasForeignKey(x => x.ObjectiveId);
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

        builder.Entity<Quest>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "Quests", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(QuestConstants.MaxNameLength);
            
            b.Property(x => x.Description)
                .HasMaxLength(QuestConstants.MaxDescriptionLength);
            b.HasOne(x => x.Milestone)
                .WithMany(x => x.Quests)
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