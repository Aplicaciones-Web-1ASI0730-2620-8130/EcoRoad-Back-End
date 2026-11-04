using System.Text;
using EcoRoad.Compliance.Domain.Model.Queries;

namespace EcoRoad.Compliance.Interfaces.Rest.Documents;

public static class ComplianceReportDocument
{
    public static byte[] Create(ComplianceReportView view)
    {
        var lines = new[]
        {
            "EcoRoad compliance report",
            "Id: " + view.Id,
            "Scope: " + view.Scope,
            "Subject: " + view.SubjectId,
            "Requested by: " + view.RequestedBy,
            "Status: " + view.Status,
            "Requested at: " + view.RequestedAt.ToString("O"),
            "Generated at: " + (view.GeneratedAt?.ToString("O") ?? string.Empty),
            "Monitoring records: " + view.MonitoringRecordIds.Count,
            "Alerts: " + view.AlertIds.Count,
            "Incidents: " + view.IncidentIds.Count,
            "Field evidence: " + view.FieldEvidenceIds.Count,
            "SHA-256: " + view.ContentHash,
            "Digital signature: " + view.DigitalSignature
        };

        return SimplePdf.Create(lines);
    }

    private static class SimplePdf
    {
        public static byte[] Create(IReadOnlyList<string> lines)
        {
            var stream = new StringBuilder();
            stream.Append("BT\n/F1 11 Tf\n50 780 Td\n14 TL\n");
            foreach (var line in lines)
            {
                stream.Append('(').Append(Escape(line)).Append(") Tj\nT*\n");
            }

            stream.Append("ET\n");
            var content = stream.ToString();
            var objects = new[]
            {
                "<< /Type /Catalog /Pages 2 0 R >>",
                "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
                "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Contents 4 0 R /Resources << /Font << /F1 5 0 R >> >> >>",
                "<< /Length " + Encoding.ASCII.GetByteCount(content) + " >>\nstream\n" + content + "endstream",
                "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>"
            };

            var builder = new StringBuilder();
            builder.Append("%PDF-1.4\n");
            var offsets = new int[objects.Length];
            for (var index = 0; index < objects.Length; index++)
            {
                offsets[index] = Encoding.ASCII.GetByteCount(builder.ToString());
                builder.Append(index + 1).Append(" 0 obj\n").Append(objects[index]).Append("\nendobj\n");
            }

            var xref = Encoding.ASCII.GetByteCount(builder.ToString());
            builder.Append("xref\n0 ").Append(objects.Length + 1).Append("\n");
            builder.Append("0000000000 65535 f \n");
            foreach (var offset in offsets)
            {
                builder.Append(offset.ToString("D10")).Append(" 00000 n \n");
            }

            builder.Append("trailer\n<< /Size ").Append(objects.Length + 1).Append(" /Root 1 0 R >>\n");
            builder.Append("startxref\n").Append(xref).Append("\n%%EOF");
            return Encoding.ASCII.GetBytes(builder.ToString());
        }

        private static string Escape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
        }
    }
}
