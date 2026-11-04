# Incident and Remediation Management

This bounded context coordinates the operational response to the environmental incidents detected
on the road works: the lifecycle of the incident ticket, the assignment of a responsible person,
the corrective actions executed by the field crews, the verifiable field evidence and the formal
closure. Alerting and Risk Evaluation keeps the original alert; the incident references its identifier.

## Domain model

| Element | Type | Responsibility |
| --- | --- | --- |
| `EnvironmentalIncident` | Aggregate root | Incident opened from an alert; owns its corrective actions and evidence. |
| `CorrectiveAction` | Entity | Remediation task with its status and progress from 0 to 100. |
| `FieldEvidence` | Entity (immutable) | Verifiable evidence with notes and an optional file address. |
| `IncidentStatus` | Value object (enum) | `Open`, `Assigned`, `Attended`, `Closed`. |
| `ActionStatus` | Value object (enum) | `Assigned`, `InProgress`, `Executed`. |
| `IncidentSeverity` | Value object (enum) | `Warning`, `Critical`, inherited from the alert. |

## Business rules

- An alert can originate only one incident.
- The responsible person can be assigned or changed while the incident is `Open` or `Assigned`; the first assignment moves it to `Assigned`.
- Corrective actions and field evidence can only be registered while the incident is `Assigned`.
- Action progress goes from 0 to 100 and never backwards. Reporting progress starts a pending action and reaching 100 executes it.
- An incident becomes `Attended` only with at least one corrective action, every action executed and at least one piece of evidence.
- Only an `Attended` incident can be closed. Evidence files must be absolute `http` or `https` addresses.
- Corrective actions and evidence are changed only through the aggregate root, which keeps the incident consistent.

## Endpoints

| Method | Route | Description |
| --- | --- | --- |
| GET | `/api/incidents?companyId=&status=&projectId=&search=` | Lists the board. |
| GET | `/api/incidents/summary?companyId=` | Number of incidents per status. |
| GET | `/api/incidents/{incidentId}` | Returns an incident with its actions and evidence. |
| POST | `/api/incidents` | Opens an incident from an alert. |
| POST | `/api/incidents/{incidentId}/assignment` | Assigns the responsible person, body `{ userId, userName }`. |
| POST | `/api/incidents/{incidentId}/actions` | Registers a corrective action, body `{ description, assignedBy }`. |
| POST | `/api/incidents/{incidentId}/actions/{actionId}/start` | Starts an action. |
| PUT | `/api/incidents/{incidentId}/actions/{actionId}/progress` | Reports progress, body `{ progressPercentage }`. |
| POST | `/api/incidents/{incidentId}/actions/{actionId}/execution` | Completes an action. |
| GET | `/api/incidents/{incidentId}/evidence` | Lists the evidence. |
| POST | `/api/incidents/{incidentId}/evidence` | Records evidence, body `{ notes, fileUrl, registeredBy }`. |
| POST | `/api/incidents/{incidentId}/resolution` | Marks the incident as attended. |
| POST | `/api/incidents/{incidentId}/closure` | Closes the incident, body `{ closedBy, notes }`. |

Errors are returned as `{ code, message }`, for example `INCIDENT_NOT_FOUND` (404),
`ALERT_ALREADY_HAS_INCIDENT`, `INVALID_STATUS` or `REMEDIATION_INCOMPLETE` (409).

## Integration notes

Once this branch is merged with IAM, the controllers can take the acting user from the session
token instead of the request body and check `ManageIncidents`, `RegisterCorrectiveActions` and
`RegisterFieldEvidence` through `IIamContextFacade.HasPermissionAsync`.

## Persistence

Tables `environmental_incidents`, `corrective_actions` and `field_evidences`, created by the
migration `InitialIncidents`. Actions and evidence are deleted in cascade with their incident.

## Local demo data

In the `Development` environment the board of `demo-company` is seeded with one open incident,
one incident under remediation and one attended incident.
