using EmsAmbulanceApp.Web.Client.Application.Data;
using EmsAmbulanceApp.Web.Client.Domain.Entities;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;

namespace EmsAmbulanceApp.Web.Client.Application.Infrastructure.Repositories;

public class OtpEntryRepository : EmsAmbulanceAppRepository<OtpEntry>, IOtpEntryRepository
{
    public OtpEntryRepository(EmsAmbulanceAppDbContext emsAmbulanceAppDbContext) : base(emsAmbulanceAppDbContext)
    {
    }
}
