using ImpactSpace.Core.Challenges;
using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Projects;
using ImpactSpace.Core.Skills;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
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
    /* Add DbSet properties for Impact Space Aggregate Roots / Entities here. */
    public DbSet<Skill> Skills { get; set; }
    public DbSet<SkillGroup> SkillGroups { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationMember> OrganizationMembers { get; set; }
    public DbSet<OrganizationMemberSkill> OrganizationMemberSkills { get; set; }
    public DbSet<OrganizationMemberChallenge> OrganizationMemberChallenges { get; set; }
    public DbSet<OrganizationMemberAction> OrganizationMemberActions { get; set; }
    public DbSet<OrganizationProfile> OrganizationProfiles { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<MilestoneVote> MilestoneVotes { get; set; }
    public DbSet<MilestoneVoteAggregate> MilestoneVoteAggregates { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Action> Actions { get; set; }
    public DbSet<ProjectCategory> ProjectCategories { get; set; }
    public DbSet<ProjectSkill> ProjectSkills { get; set; }
    public DbSet<Objective> Objectives { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ProjectTag> ProjectTags { get; set; }
    public DbSet<Challenge> Challenges { get; set; }

    public DbSet<ProjectChallenge> ProjectChallenges { get; set; }

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

        /* Configure Impact Space tables/entities */
        builder.ConfigureSkills();
        builder.ConfigureOrganizations();
        builder.ConfigureProjects();
        builder.ConfigureChallenges();
    }
}
