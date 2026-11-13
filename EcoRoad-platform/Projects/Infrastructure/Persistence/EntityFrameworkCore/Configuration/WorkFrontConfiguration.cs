using EcoRoad.Projects.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Projects.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class WorkFrontConfiguration : IEntityTypeConfiguration<WorkFront>
{
    public void Configure(EntityTypeBuilder<WorkFront> builder)
    {
        builder.ToTable("work_fronts");

        builder.HasKey(workFront => workFront.Id);

        builder.Property(workFront => workFront.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(workFront => workFront.SectionId)
            .HasColumnName("section_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(workFront => workFront.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(workFront => workFront.Status)
            .HasColumnName("status")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.HasOne(workFront => workFront.Section)
            .WithMany(section => section.WorkFronts)
            .HasForeignKey(workFront => workFront.SectionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
