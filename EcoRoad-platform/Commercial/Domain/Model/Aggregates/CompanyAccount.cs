using EcoRoad.Commercial.Domain.Model;
using EcoRoad.Commercial.Domain.Model.ValueObjects;

namespace EcoRoad.Commercial.Domain.Model.Aggregates;

public class CompanyAccount
{
    public CompanyAccount()
    {
    }

    public CompanyAccount(string name, string code, CompanySegment segment)
    {
        Id = Guid.NewGuid().ToString();
        Name = CommercialRules.Require(name, "Name", 100);
        Code = CommercialRules.RequireCompanyCode(code);
        Segment = segment;
        Status = CompanyAccountStatus.Registered;
        CreatedAt = DateTime.UtcNow;
    }

    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public CompanySegment Segment { get; set; }

    public CompanyAccountStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool HasPlatformAccess => Status == CompanyAccountStatus.Active;

    public void EnableAccess()
    {
        if (Status == CompanyAccountStatus.Active)
        {
            return;
        }

        Status = CompanyAccountStatus.Active;
    }
}
