namespace EcoRoad.Assets.Domain.Model.Queries;

/// <summary>
/// Read model with the deployment indicators of the sensor fleet.
/// </summary>
/// <param name="Total">Number of sensors.</param>
/// <param name="ReadyToReport">Sensors that are active, installed and calibrated.</param>
/// <param name="Assigned">Sensors installed but not active.</param>
/// <param name="Available">Sensors in stock.</param>
/// <param name="CalibrationDue">Installed sensors without a valid calibration.</param>
public record AssetSummary(int Total, int ReadyToReport, int Assigned, int Available, int CalibrationDue);
