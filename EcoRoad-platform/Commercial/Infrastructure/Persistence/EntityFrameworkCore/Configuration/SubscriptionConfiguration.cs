using EcoRoad.Commercial.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Commercial.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("subscriptions");

        builder.HasKey(subscription => subscription.Id);

        builder.Property(subscription => subscription.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(subscription => subscription.CompanyAccountId)
            .HasColumnName("company_account_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(subscription => subscription.Plan)
            .HasColumnName("plan")
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(subscription => subscription.BillingCycle)
            .HasColumnName("billing_cycle")
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(subscription => subscription.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(subscription => subscription.SelectedAt)
            .HasColumnName("selected_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(subscription => subscription.ActivatedAt)
            .HasColumnName("activated_at")
            .HasColumnType("datetime")
            .IsRequired(false);

        builder.Property(subscription => subscription.CurrentPeriodEnd)
            .HasColumnName("current_period_end")
            .HasColumnType("datetime")
            .IsRequired(false);

        builder.HasOne<CompanyAccount>()
            .WithMany()
            .HasForeignKey(subscription => subscription.CompanyAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(subscription => subscription.CompanyAccountId)
            .IsUnique();
    }
}
