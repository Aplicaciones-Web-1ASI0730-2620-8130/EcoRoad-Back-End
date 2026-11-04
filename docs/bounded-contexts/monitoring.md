# Environmental Monitoring

This bounded context captures and processes the environmental telemetry of the road works (air,
noise, water and vibration). It configures the threshold profiles of every parameter and classifies
each automatic or manual measurement when it is recorded, so the evidence cannot be altered later.
Alerting and Risk Evaluation and Compliance and Reporting consume these measurements.

## Domain model

| Element | Type | Responsibility |
| --- | --- | --- |
| `ThresholdProfile` | Aggregate root | Limits of a parameter, either a maximum (warning and critical) or an optimal range. |
| `EnvironmentalMeasurement` | Aggregate root | Immutable reading captured by a sensor or recorded manually, with its status. |
| `ParameterType` | Value object (enum) | `AirQuality` (PM10, ug/m3), `NoiseLevel` (LAeq, dBA), `WaterQuality` (pH), `Vibration` (PPV, mm/s). |
| `ThresholdLimits` | Value object | Shape and values of the limits of a profile. |
| `MonitoringStatus` | Value object (enum) | `Optimal`, `Observation`, `Critical`. |
| `MeasurementSource` | Value object (enum) | `AutomaticSensor`, `ManualEntry`. |
| `MonitoringDashboardBuilder` | Domain service | Builds the project dashboard from the latest reading of every section and parameter. |

## Business rules

- Every parameter has at most one threshold profile, and a measurement cannot be recorded until its parameter has one.
- Maximum profiles require `warningThreshold < criticalThreshold`; range profiles require `minimum <= optimalMinimum < optimalMaximum <= maximum`.
- A maximum value is `Critical` from the critical threshold and `Observation` from the warning threshold. A range value is `Critical` outside `[minimum, maximum]` and `Observation` outside the optimal band.
- Values must be physically possible (non negative, pH up to 14) and the measurement date cannot be in the future (5 minutes of clock tolerance).
- Automatic readings require the monitoring point and the sensor; manual records require the operator.
- Measurements are immutable and keep the status and the profile used when they were recorded, even if the thresholds change later.
- The dashboard considers the readings of the last 30 days. History filters require `from <= to`.

## Endpoints

| Method | Route | Description |
| --- | --- | --- |
| GET | `/api/monitoring/threshold-profiles` | Lists the profiles. |
| GET | `/api/monitoring/threshold-profiles/{profileId}` | Returns a profile. |
| POST | `/api/monitoring/threshold-profiles` | Configures a profile, body `{ name, parameterType, limits }`. |
| PUT | `/api/monitoring/threshold-profiles/{profileId}/thresholds` | Replaces the limits of a profile. |
| POST | `/api/monitoring/measurements/telemetry` | Captures a sensor reading. |
| POST | `/api/monitoring/measurements/manual` | Records a manual field measurement. |
| GET | `/api/monitoring/measurements/{measurementId}` | Returns a measurement. |
| GET | `/api/monitoring/projects/{projectId}/dashboard` | Indicators, current readings and status per section. |
| GET | `/api/monitoring/projects/{projectId}/history?parameterType=&sectionId=&from=&to=` | History of a parameter. |

Errors are returned as `{ code, message }`, for example `PROFILE_NOT_CONFIGURED` (409),
`INVALID_MEASUREMENT` (422) or `INVALID_HISTORY_FILTERS` (422).

## Integration notes

Once this branch is merged with Monitoring Asset and Deployment, the telemetry endpoint should call
`IAssetsContextFacade.CanSensorReportAsync` so only active and calibrated sensors are accepted.

## Persistence

Tables `threshold_profiles` and `environmental_measurements`, created by the migration `InitialMonitoring`.

## Local demo data

In the `Development` environment four illustrative threshold profiles (not an official regulatory
profile) and the demo readings of the projects `prj-lima-canta`, `prj-arequipa-norte` and
`prj-cusco-anta` are seeded.
