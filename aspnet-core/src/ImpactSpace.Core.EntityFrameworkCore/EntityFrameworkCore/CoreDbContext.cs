using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Projects;
using ImpactSpace.Core.Skills;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace ImpactSpace.Core.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class CoreDbContext :
    AbpDbContext<CoreDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    public DbSet<Skill> Skills { get; set; }
    public DbSet<SkillGroup> SkillGroups { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationMember> OrganizationMembers { get; set; }
    public DbSet<OrganizationMemberSkill> OrganizationMemberSkills { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<MilestoneVote> MilestoneVotes { get; set; }
    public DbSet<MilestoneVoteAggregate> MilestoneVoteAggregates { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Activity> ProjectActions { get; set; }
    public DbSet<ProjectCategory> ProjectCategories { get; set; }
    public DbSet<ProjectSkill> ProjectSkills { get; set; }
    public DbSet<Quest> Quests { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public CoreDbContext(DbContextOptions<CoreDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */
        ConfigureSkills(builder);
        ConfigureOrganizations(builder);
        ConfigureProjects(builder);
    }

    private void ConfigureProjects(ModelBuilder builder)
    {
        builder.Entity<MilestoneVoteAggregate>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "MilestoneVoteAggregates", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.HasOne(x => x.Milestone)
                .WithMany()
                .HasForeignKey(x => x.MilestoneId);
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
        });

        builder.Entity<Activity>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "Activities", CoreConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ActivityConstants.MaxNameLength);
            b.Property(x => x.Description)
                .HasMaxLength(ActivityConstants.MaxDescriptionLength);
            
            b.HasOne(x => x.Quest)
                .WithMany(x => x.Activities)
                .HasForeignKey(x => x.QuestId);
            b.HasMany(x => x.OrganizationMemberActivities)
                .WithOne(x => x.Activity)
                .HasForeignKey(x => x.ActivityId);
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
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
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

    private void ConfigureOrganizations(ModelBuilder builder)
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
            b.HasMany(x => x.OrganizationMemberActivities)
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
        
        builder.Entity<OrganizationMemberActivity>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "OrganizationMemberActivities", CoreConsts.DbSchema);
            b.ConfigureByConvention();

            b.HasKey(x => new { x.OrganizationMemberId, x.ActivityId });

            b.HasOne(x => x.OrganizationMember)
                .WithMany(x => x.OrganizationMemberActivities)
                .HasForeignKey(x => x.OrganizationMemberId);

            b.HasOne(x => x.Activity)
                .WithMany(x => x.OrganizationMemberActivities)
                .HasForeignKey(x => x.ActivityId);
        });
    }

    private static void ConfigureSkills(ModelBuilder builder)
    {
        builder.Entity<Skill>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "Skills", CoreConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(SkillConstants.MaxNameLength);
            b.HasIndex(x => x.Name).IsUnique();
            
            b.HasMany(x => x.OrganizationMemberSkills)
                .WithOne()
                .HasForeignKey(x => x.SkillId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(x => x.ProjectSkills)
                .WithOne()
                .HasForeignKey(x => x.SkillId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<SkillGroup>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "SkillGroups", CoreConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(SkillGroupConstants.MaxNameLength);
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Description)
                .HasMaxLength(SkillGroupConstants.MaxDescriptionLength);

            // Configure one-to-many relationship between SkillGroup and Skill
            b.HasMany(sg => sg.Skills)
                .WithOne()
                .HasForeignKey(s => s.SkillGroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
