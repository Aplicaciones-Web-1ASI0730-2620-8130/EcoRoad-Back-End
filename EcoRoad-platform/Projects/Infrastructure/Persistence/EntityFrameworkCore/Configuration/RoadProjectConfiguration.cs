using EcoRoad.Projects.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Projects.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class RoadProjectConfiguration : IEntityTypeConfiguration<RoadProject>
{
    public void Configure(EntityTypeBuilder<RoadProject> builder)
    {
        builder.ToTable("road_projects");

        builder.HasKey(project => project.Id);

        builder.Property(project => project.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(project => project.CompanyId)
            .HasColumnName("company_id")
            .HasColumnType("varchar(3)")
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(project => project.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(project => project.Code)
            .HasColumnName("code")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(project => project.Status)
            .HasColumnName("status")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(project => project.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime")
            .IsRequired();
    }
}
