using EmsAmbulanceApp.Web.Client.Application.Data;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EmsAmbulanceApp.Web.Client.Application.Infrastructure.Repositories;

public class ClientRepository : EmsAmbulanceAppRepository<Domain.Entities.Client>, IClientRepository
{
    public ClientRepository(EmsAmbulanceAppDbContext emsAmbulanceAppDbContext) : base(emsAmbulanceAppDbContext)
    {
    }

    public async Task<Domain.Entities.Client?> FindByPhoneNumberAsync(string? phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
        {
            throw new ArgumentNullException(nameof(phoneNumber));
        }

        return await _emsAmbulanceAppDbContext.Clients.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
    }
}
