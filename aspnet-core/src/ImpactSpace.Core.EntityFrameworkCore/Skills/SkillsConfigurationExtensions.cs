using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ImpactSpace.Core.Skills;

public static class SkillsConfigurationExtensions
{
    public static void ConfigureSkills(this ModelBuilder builder)
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
                .IsRequired();

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
                .IsRequired();
        });
    }
}