using EmsAmbulanceApp.Web.Client.Application.Data;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;

namespace EmsAmbulanceApp.Web.Client.Application.Infrastructure.Repositories;

public class AmbulanceRequestRepository : EmsAmbulanceAppRepository<Domain.Entities.AmbulanceRequest>, IAmbulanceRequestRepository
{
    public AmbulanceRequestRepository(EmsAmbulanceAppDbContext emsAmbulanceAppDbContext) : base(emsAmbulanceAppDbContext)
    {
    }
}
