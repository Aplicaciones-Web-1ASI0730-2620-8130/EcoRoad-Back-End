using EcoRoad.Incidents.Domain.Model.Aggregates;
using EcoRoad.Incidents.Domain.Model.ValueObjects;
using EcoRoad.Incidents.Domain.Repositories;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Incidents.Application.Internal.CommandServices;

/// <summary>
/// Seeds the demo incident board used by the frontend during local development.
/// </summary>
/// <remarks>
/// The three cases show the board columns: one open, one under remediation and one attended.
/// They are built through the aggregate methods, so the demo data respects every rule.
/// </remarks>
public class IncidentsSampleDataSeeder
{
    private const string DemoCompanyId = "demo-company";
    private const string AdministratorId = "usr-carlos";

    private readonly IEnvironmentalIncidentRepository _incidents;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<IncidentsSampleDataSeeder> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IncidentsSampleDataSeeder" /> class.
    /// </summary>
    /// <param name="incidents">Repository of incidents.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the seeding.</param>
    public IncidentsSampleDataSeeder(
        IEnvironmentalIncidentRepository incidents,
        IUnitOfWork unitOfWork,
        ILogger<IncidentsSampleDataSeeder> logger)
    {
        _incidents = incidents;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Creates the demo incidents when the board is empty.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    public async Task SeedIfEmptyAsync(CancellationToken cancellationToken = default)
    {
        if ((await _incidents.ListAsync(cancellationToken)).Count > 0)
        {
            return;
        }

        var now = DateTime.UtcNow;

        var particulate = EnvironmentalIncident.OpenEnvironmentalIncident(
            DemoCompanyId,
            "ALR-024",
            "prj-lima-canta",
            "lim-03",
            "PM10 above the critical threshold",
            "High concentration of particulate matter at the cutting front.",
            IncidentSeverity.Critical,
            AdministratorId,
            now.AddHours(-2));

        var noise = EnvironmentalIncident.OpenEnvironmentalIncident(
            DemoCompanyId,
            "ALR-021",
            "prj-lima-canta",
            "lim-02",
            "Noise close to the limit",
            "Noise level close to the threshold next to the crushing plant.",
            IncidentSeverity.Warning,
            AdministratorId,
            now.AddHours(-20));
        noise.AssignResponsiblePerson("usr-luis", "Luis Garcia", now.AddHours(-19));
        var barriers = noise.AssignCorrectiveAction("Install mobile acoustic barriers next to the crusher.", AdministratorId, now.AddHours(-18));
        noise.UpdateRemediationProgress(barriers.Id, 60, now.AddHours(-6));
        noise.RecordFieldEvidence("Photographs of the installed barriers and a control measurement.", null, "usr-luis", now.AddHours(-5));

        var water = EnvironmentalIncident.OpenEnvironmentalIncident(
            DemoCompanyId,
            "ALR-LEGACY-019",
            "prj-lima-canta",
            "lim-04",
            "pH deviation",
            "Preventive pH deviation at the sampling point.",
            IncidentSeverity.Warning,
            AdministratorId,
            now.AddDays(-3));
        water.AssignResponsiblePerson("usr-ana", "Ana Torres", now.AddDays(-3).AddHours(1));
        var resampling = water.AssignCorrectiveAction("Repeat the sampling and adjust the runoff barriers.", AdministratorId, now.AddDays(-3).AddHours(2));
        water.ExecuteCorrectiveAction(resampling.Id, now.AddDays(-2));
        water.RecordFieldEvidence("Documented field counter sample.", null, "usr-luis", now.AddDays(-2));
        water.MarkIncidentAsAttended(now.AddDays(-2).AddHours(1));

        await _incidents.AddAsync(particulate, cancellationToken);
        await _incidents.AddAsync(noise, cancellationToken);
        await _incidents.AddAsync(water, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Incident demo board seeded.");
    }
}
