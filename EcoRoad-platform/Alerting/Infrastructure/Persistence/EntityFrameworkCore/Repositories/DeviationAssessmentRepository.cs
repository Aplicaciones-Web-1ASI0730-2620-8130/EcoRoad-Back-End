using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for deviation assessments.
/// </summary>
public class DeviationAssessmentRepository : IDeviationAssessmentRepository
{
    private readonly AppDbContext _context;

    public DeviationAssessmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DeviationAssessment?> FindByIdAsync(
        string assessmentId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<DeviationAssessment>()
            .AsNoTracking()
            .FirstOrDefaultAsync(assessment => assessment.Id == assessmentId, cancellationToken);
    }

    public async Task<IReadOnlyList<DeviationAssessment>> ListAsync(
        string? measurementId,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<DeviationAssessment>().AsNoTracking();
        if (!string.IsNullOrWhiteSpace(measurementId))
        {
            query = query.Where(assessment => assessment.MeasurementId == measurementId);
        }

        return await query
            .OrderByDescending(assessment => assessment.AssessedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(DeviationAssessment assessment, CancellationToken cancellationToken = default)
    {
        await _context.Set<DeviationAssessment>().AddAsync(assessment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(DeviationAssessment assessment, CancellationToken cancellationToken = default)
    {
        _context.Set<DeviationAssessment>().Update(assessment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
