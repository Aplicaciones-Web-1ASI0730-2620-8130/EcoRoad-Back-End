using EcoRoad.Projects.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Projects.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class RoadSectionConfiguration : IEntityTypeConfiguration<RoadSection>
{
    public void Configure(EntityTypeBuilder<RoadSection> builder)
    {
        builder.ToTable("road_sections");

        builder.HasKey(section => section.Id);

        builder.Property(section => section.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(section => section.ProjectId)
            .HasColumnName("project_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(section => section.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(section => section.StartKilometer)
            .HasColumnName("start_kilometer")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(section => section.EndKilometer)
            .HasColumnName("end_kilometer")
            .HasColumnType("double")
            .IsRequired();

        builder.HasOne(section => section.Project)
            .WithMany(project => project.Sections)
            .HasForeignKey(section => section.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
