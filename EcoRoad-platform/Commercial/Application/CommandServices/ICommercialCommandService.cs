using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Commands;

namespace EcoRoad.Commercial.Application.CommandServices;

public interface ICommercialCommandService
{
    Task<CompanyAccount> Handle(RegisterCompanyAccountCommand command, CancellationToken cancellationToken = default);

    Task<Subscription> Handle(SelectSubscriptionPlanCommand command, CancellationToken cancellationToken = default);

    Task<Subscription> Handle(ActivateSubscriptionCommand command, CancellationToken cancellationToken = default);

    Task<Subscription> Handle(RenewSubscriptionCommand command, CancellationToken cancellationToken = default);
}
