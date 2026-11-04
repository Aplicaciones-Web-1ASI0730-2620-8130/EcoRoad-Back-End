using EcoRoad.Incidents.Domain.Model.Aggregates;
using EcoRoad.Incidents.Domain.Model.ValueObjects;
using EcoRoad.Incidents.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IEnvironmentalIncidentRepository" />.
/// </summary>
public class EnvironmentalIncidentRepository : BaseRepository<EnvironmentalIncident>, IEnvironmentalIncidentRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentalIncidentRepository" /> class.
    /// </summary>
    /// <param name="context">The shared application database context.</param>
    public EnvironmentalIncidentRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <summary>
    /// Loads the whole aggregate, including its corrective actions and field evidence.
    /// </summary>
    /// <param name="id">Identifier of the incident.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The incident when it exists; otherwise <c>null</c>.</returns>
    public override async Task<EnvironmentalIncident?> FindByIdAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        return await WithDetails(Context.Set<EnvironmentalIncident>())
            .FirstOrDefaultAsync(incident => incident.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByAlertIdAsync(string alertId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<EnvironmentalIncident>().AnyAsync(incident => incident.AlertId == alertId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<EnvironmentalIncident>> ListByFiltersAsync(
        string? companyId,
        IncidentStatus? status,
        string? projectId,
        string? search,
        CancellationToken cancellationToken = default)
    {
        var query = WithDetails(Context.Set<EnvironmentalIncident>().AsNoTracking());

        if (!string.IsNullOrWhiteSpace(companyId))
        {
            query = query.Where(incident => incident.CompanyId == companyId);
        }

        if (status.HasValue)
        {
            query = query.Where(incident => incident.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(projectId))
        {
            query = query.Where(incident => incident.ProjectId == projectId);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(incident => incident.Code.Contains(term) || incident.Title.Contains(term));
        }

        var incidents = await query.ToListAsync(cancellationToken);

        // Enumerations are stored as text, so the board order is applied in memory.
        return incidents
            .OrderBy(incident => incident.Status)
            .ThenByDescending(incident => incident.OpenedAt)
            .ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyDictionary<IncidentStatus, int>> CountByStatusAsync(
        string? companyId,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<EnvironmentalIncident>().AsNoTracking();
        if (!string.IsNullOrWhiteSpace(companyId))
        {
            query = query.Where(incident => incident.CompanyId == companyId);
        }

        var counts = await query
            .GroupBy(incident => incident.Status)
            .Select(group => new { Status = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        return counts.ToDictionary(entry => entry.Status, entry => entry.Count);
    }

    /// <summary>
    /// Adds the child collections of the aggregate to a query.
    /// </summary>
    /// <param name="query">The base query.</param>
    /// <returns>The query including actions and evidence.</returns>
    private static IQueryable<EnvironmentalIncident> WithDetails(IQueryable<EnvironmentalIncident> query)
    {
        return query
            .Include(incident => incident.CorrectiveActions)
            .Include(incident => incident.FieldEvidences)
            .AsSplitQuery();
    }
}
