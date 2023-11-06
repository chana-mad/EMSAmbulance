namespace EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;

public interface IClientRepository : IEmsAmbulanceAppRepository<Entities.Client>
{
    Task<Entities.Client?> FindByPhoneNumberAsync(string phoneNumber);
}
