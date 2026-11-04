namespace EcoRoad.Shared.Interfaces.Rest.Resources;

/// <summary>
/// Error body returned by every endpoint when a request cannot be completed.
/// </summary>
/// <param name="Code">Stable, machine readable error code, for example <c>SENSOR_NOT_FOUND</c>.</param>
/// <param name="Message">Human readable description of the error.</param>
public record ErrorResource(string Code, string Message);
