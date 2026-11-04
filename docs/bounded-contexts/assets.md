# Monitoring Asset and Deployment

This bounded context manages the IoT sensors that EcoRoad provides under the hardware as a
service model and the geolocated monitoring points of the road sections where they are installed.
Projects and sections belong to Project and Road Site Management and are referenced by identifier;
measurements belong to Environmental Monitoring, which asks this context whether a sensor can report
through `IAssetsContextFacade`.

## Domain model

| Element | Type | Responsibility |
| --- | --- | --- |
| `MonitoringPoint` | Aggregate root | Geolocated point of a road section, identified by its kilometer post. |
| `Sensor` | Aggregate root | Device of the fleet with its deployment lifecycle and calibration. |
| `SensorAssignment` | Aggregate root | Installation history of every sensor at every point. |
| `KilometerPost` | Value object | Kilometer post such as `PK 45+100`, normalized to `45+100`. |
| `SensorType` | Value object (enum) | `Pm10`, `Noise`, `Water`, `Vibration`. |
| `SensorStatus` | Value object (enum) | `Available`, `Assigned`, `Active`. |
| `MonitoringPointStatus` | Value object (enum) | `Enabled`, `Disabled`. |

## Business rules

- Kilometer posts follow the format `PK 00+000` and coordinates must be valid latitudes and longitudes.
- Point names are unique inside a road section. Serial numbers are unique across the fleet.
- Lifecycle: `Available`, assignment, `Assigned`, calibration, activation, `Active`. Deactivation returns the sensor to `Assigned`; release returns it to `Available`.
- A point hosts at most one sensor of each type, and only enabled points receive sensors.
- Active sensors must be deactivated before being reassigned or released. Reassignment and release invalidate the calibration.
- Calibrations must be valid until a future date. Activation requires a valid calibration.
- Reporting policy (`canReport`): the sensor is active, installed at a point and its calibration is still valid.
- Points with installed sensors cannot be moved, disabled or deleted. Points that ever hosted a sensor are disabled instead of deleted, so the measurement history stays traceable.

## Endpoints

| Method | Route | Description |
| --- | --- | --- |
| GET | `/api/assets/summary?projectId=` | Totals of sensors ready to report, assigned, available and with calibration due. |
| GET | `/api/assets/points?projectId=` | Lists monitoring points. |
| GET | `/api/assets/points/{pointId}` | Returns a monitoring point. |
| POST | `/api/assets/points` | Registers a point, body `{ projectId, sectionId, name, kilometerPost, latitude, longitude }`. |
| PUT | `/api/assets/points/{pointId}` | Updates a point without sensors. |
| DELETE | `/api/assets/points/{pointId}` | Deletes a point without history. |
| POST | `/api/assets/points/{pointId}/enablement` | Enables a point. |
| POST | `/api/assets/points/{pointId}/disablement` | Disables a point without sensors. |
| GET | `/api/assets/sensors?projectId=&status=&search=` | Lists the inventory. |
| GET | `/api/assets/sensors/{sensorId}` | Returns a sensor with `canReport`. |
| GET | `/api/assets/sensors/{sensorId}/assignments` | Installation history. |
| POST | `/api/assets/sensors` | Registers a sensor, body `{ name, serialNumber, type, model }`. |
| POST | `/api/assets/sensors/{sensorId}/assignment` | Installs the sensor, body `{ monitoringPointId }`. |
| POST | `/api/assets/sensors/{sensorId}/calibration` | Registers a calibration, body `{ validUntil }`. |
| POST | `/api/assets/sensors/{sensorId}/activation` | Activates the sensor. |
| POST | `/api/assets/sensors/{sensorId}/deactivation` | Deactivates the sensor. |
| POST | `/api/assets/sensors/{sensorId}/release` | Releases the sensor to stock. |

Errors are returned as `{ code, message }`, for example `SENSOR_NOT_FOUND` (404), `INVALID_POINT`
(422), `DUPLICATE_SERIAL`, `POINT_TYPE_OCCUPIED`, `POINT_IN_USE` or `CALIBRATION_REQUIRED` (409).

## Persistence

Tables `monitoring_points`, `sensors` and `sensor_assignments`, created by the migration `InitialAssets`.

## Local demo data

In the `Development` environment four points of the demo projects `prj-lima-canta` and
`prj-arequipa-norte` are seeded with three active sensors, one assigned sensor pending calibration
and one sensor in stock.
