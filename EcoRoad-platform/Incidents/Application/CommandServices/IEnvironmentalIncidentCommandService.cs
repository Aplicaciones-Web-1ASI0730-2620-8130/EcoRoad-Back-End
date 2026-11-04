using EcoRoad.Incidents.Domain.Model.Aggregates;
using EcoRoad.Incidents.Domain.Model.Commands;
using EcoRoad.Incidents.Domain.Model.Entities;

namespace EcoRoad.Incidents.Application.CommandServices;

/// <summary>
/// Application service that handles the lifecycle of the <see cref="EnvironmentalIncident" /> aggregate.
/// </summary>
public interface IEnvironmentalIncidentCommandService
{
    /// <summary>Opens an incident from an alert.</summary>
    /// <param name="command">The open command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The open incident.</returns>
    Task<EnvironmentalIncident> Handle(OpenIncidentCommand command, CancellationToken cancellationToken = default);

    /// <summary>Assigns the responsible person.</summary>
    /// <param name="command">The assignment command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated incident.</returns>
    Task<EnvironmentalIncident> Handle(AssignResponsibleCommand command, CancellationToken cancellationToken = default);

    /// <summary>Records field evidence.</summary>
    /// <param name="command">The evidence command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The recorded evidence.</returns>
    Task<FieldEvidence> Handle(RecordFieldEvidenceCommand command, CancellationToken cancellationToken = default);

    /// <summary>Marks the incident as attended.</summary>
    /// <param name="command">The attention command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The updated incident.</returns>
    Task<EnvironmentalIncident> Handle(MarkIncidentAsAttendedCommand command, CancellationToken cancellationToken = default);

    /// <summary>Closes the incident.</summary>
    /// <param name="command">The closure command.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The closed incident.</returns>
    Task<EnvironmentalIncident> Handle(CloseIncidentCommand command, CancellationToken cancellationToken = default);
}
