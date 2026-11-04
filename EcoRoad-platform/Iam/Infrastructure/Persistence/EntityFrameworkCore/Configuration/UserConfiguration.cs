using EcoRoad.Iam.Domain.Model.Aggregates;
using EcoRoad.Iam.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoRoad.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Maps the <see cref="User" /> aggregate to the <c>users</c> table.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasColumnName("id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(user => user.CompanyId)
            .HasColumnName("company_id")
            .HasColumnType("varchar(36)")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(user => user.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.Email)
            .HasColumnName("email")
            .HasColumnType("varchar(120)")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasColumnName("password_hash")
            .HasColumnType("varchar(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(user => user.Role)
            .HasColumnName("role")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        // Permissions are stored as a comma separated list of names, for example "ViewProjects,ConsultAlerts".
        builder.Property(user => user.Permissions)
            .HasColumnName("permissions")
            .HasColumnType("varchar(500)")
            .HasMaxLength(500)
            .HasConversion(
                permissions => SerializePermissions(permissions),
                value => DeserializePermissions(value),
                new ValueComparer<IReadOnlyList<Permission>>(
                    (left, right) => left!.SequenceEqual(right!),
                    permissions => permissions.Aggregate(0, (hash, permission) => HashCode.Combine(hash, permission)),
                    permissions => permissions.ToList()))
            .IsRequired();

        builder.Property(user => user.AccessStatus)
            .HasColumnName("access_status")
            .HasColumnType("varchar(30)")
            .HasMaxLength(30)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(user => user.GrantedAt)
            .HasColumnName("granted_at")
            .HasColumnType("datetime");

        builder.Property(user => user.RevokedAt)
            .HasColumnName("revoked_at")
            .HasColumnType("datetime");

        builder.Property(user => user.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(user => user.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        builder.HasIndex(user => new { user.CompanyId, user.Email })
            .IsUnique();

        builder.HasIndex(user => user.Email);
    }

    /// <summary>
    /// Converts the permissions into their stored representation.
    /// </summary>
    /// <param name="permissions">The permissions of the user.</param>
    /// <returns>A comma separated list of permission names.</returns>
    private static string SerializePermissions(IReadOnlyList<Permission> permissions)
    {
        return string.Join(',', permissions.Select(permission => permission.ToString()));
    }

    /// <summary>
    /// Restores the permissions from their stored representation, ignoring unknown names.
    /// </summary>
    /// <param name="value">A comma separated list of permission names.</param>
    /// <returns>The permissions of the user.</returns>
    private static IReadOnlyList<Permission> DeserializePermissions(string value)
    {
        return value
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(name => Enum.TryParse<Permission>(name, out var permission) ? permission : (Permission?)null)
            .Where(permission => permission.HasValue)
            .Select(permission => permission!.Value)
            .ToList();
    }
}
