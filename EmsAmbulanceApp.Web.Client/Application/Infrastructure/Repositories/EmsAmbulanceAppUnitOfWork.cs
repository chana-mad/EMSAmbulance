using EmsAmbulanceApp.Web.Client.Application.Data;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;

namespace EmsAmbulanceApp.Web.Client.Application.Infrastructure.Repositories;

public class EmsAmbulanceAppUnitOfWork : IEmsAmbulanceAppUnitOfWork
{
    private readonly EmsAmbulanceAppDbContext _dbContext;
    public IOtpEntryRepository OtpEntry { get; }
    public IClientRepository Client { get; }

    public EmsAmbulanceAppUnitOfWork(EmsAmbulanceAppDbContext dbContext)
    {
        _dbContext = dbContext;

        OtpEntry = new OtpEntryRepository(_dbContext);
        Client = new ClientRepository(_dbContext);
    }

    public async Task<int> Complete()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
