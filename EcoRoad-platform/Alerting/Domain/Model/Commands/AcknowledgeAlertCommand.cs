namespace EcoRoad.Alerting.Domain.Model.Commands;

public record AcknowledgeAlertCommand(string AlertId, string AcknowledgedBy);
