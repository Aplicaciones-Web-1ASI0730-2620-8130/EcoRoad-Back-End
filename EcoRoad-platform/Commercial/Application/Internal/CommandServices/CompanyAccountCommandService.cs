using EcoRoad.Commercial.Application.CommandServices;
using EcoRoad.Commercial.Domain.Model;
using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Model.Commands;
using EcoRoad.Commercial.Domain.Repositories;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Commercial.Application.Internal.CommandServices;

public class CompanyAccountCommandService : ICompanyAccountCommandService
{
    private readonly ICompanyAccountRepository _accounts;
    private readonly IUnitOfWork _unitOfWork;

    public CompanyAccountCommandService(ICompanyAccountRepository accounts, IUnitOfWork unitOfWork)
    {
        _accounts = accounts;
        _unitOfWork = unitOfWork;
    }

    public async Task<CompanyAccount> Handle(
        RegisterCompanyAccountCommand command,
        CancellationToken cancellationToken = default)
    {
        var account = new CompanyAccount(command.Name, command.Code, command.Segment);
        var existing = await _accounts.GetByCodeAsync(account.Code, cancellationToken);
        if (existing != null)
        {
            throw new CommercialRuleException("A company account with this code is already registered.");
        }

        await _accounts.AddAsync(account, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return account;
    }
}
