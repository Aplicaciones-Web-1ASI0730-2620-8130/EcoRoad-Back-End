using EcoRoad.Commercial.Domain.Model.Aggregates;
using EcoRoad.Commercial.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcoRoad.Commercial.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class CompanyAccountRepository : ICompanyAccountRepository
{
    private readonly AppDbContext _context;

    public CompanyAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyAccount?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<CompanyAccount>()
            .AsNoTracking()
            .FirstOrDefaultAsync(account => account.Id == id, cancellationToken);
    }

    public async Task<CompanyAccount?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Set<CompanyAccount>()
            .AsNoTracking()
            .FirstOrDefaultAsync(account => account.Code == code, cancellationToken);
    }

    public async Task AddAsync(CompanyAccount account, CancellationToken cancellationToken = default)
    {
        await _context.Set<CompanyAccount>().AddAsync(account, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(CompanyAccount account, CancellationToken cancellationToken = default)
    {
        _context.Set<CompanyAccount>().Update(account);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
