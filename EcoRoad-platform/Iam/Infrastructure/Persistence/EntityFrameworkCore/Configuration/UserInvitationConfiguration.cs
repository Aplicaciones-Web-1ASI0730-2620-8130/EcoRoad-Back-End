using EcoRoad.Iam.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Maps the <see cref="UserInvitation" /> aggregate to the <c>user_invitations</c> table.
/// </summary>
public class UserInvitationConfiguration : IEntityTypeConfiguration<UserInvitation>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UserInvitation> builder)
    {
        builder.ToTable("user_invitations");

        builder.HasKey(invitation => invitation.Id);

        builder.Property(invitation => invitation.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(invitation => invitation.CompanyId)
            .HasColumnName("company_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(invitation => invitation.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(invitation => invitation.Email)
            .HasColumnName("email")
            .HasColumnType("varchar(120)")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(invitation => invitation.Role)
            .HasColumnName("role")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(invitation => invitation.Status)
            .HasColumnName("status")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(invitation => invitation.InvitedByUserId)
            .HasColumnName("invited_by_user_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(invitation => invitation.ExpiresAt)
            .HasColumnName("expires_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(invitation => invitation.AcceptedAt)
            .HasColumnName("accepted_at")
            .HasColumnType("datetime");

        builder.Property(invitation => invitation.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(invitation => invitation.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        builder.HasIndex(invitation => new { invitation.CompanyId, invitation.Email, invitation.Status });
    }
}
