using EcoRoad.Assets.Domain.Model.ValueObjects;

namespace EcoRoad.Assets.Domain.Model.Commands;

/// <summary>
/// Registers a new sensor in the stock of the fleet.
/// </summary>
/// <param name="Name">Descriptive name of the device.</param>
/// <param name="SerialNumber">Manufacturer serial number.</param>
/// <param name="Type">Environmental variable the sensor captures.</param>
/// <param name="Model">Optional manufacturer model.</param>
public record RegisterSensorCommand(string Name, string SerialNumber, SensorType Type, string? Model);
