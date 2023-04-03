using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Projects;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ImpactSpace.Core.Challenges;

public static class ChallengesConfigurationExtensions
{
    public static void ConfigureChallenges(this ModelBuilder builder)
    {
        builder.Entity<Challenge>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "Challenges", CoreConsts.DbSchema);
            b.ConfigureByConvention();
                
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ChallengeConstants.MaxNameLength);
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

        builder.Entity<ProjectChallenge>(b =>
        {
            b.ToTable(CoreConsts.DbTablePrefix + "ProjectChallenges", CoreConsts.DbSchema);
            b.ConfigureByConvention();

            b.HasKey(x => new { x.ProjectId, x.ChallengeId });

            b.HasOne(x => x.Project)
                .WithMany(x => x.ProjectChallenges)
                .HasForeignKey(x => x.ProjectId);

            b.HasOne(x => x.Challenge)
                .WithMany(x => x.ProjectChallenges)
                .HasForeignKey(x => x.ChallengeId);
        });
    }
}