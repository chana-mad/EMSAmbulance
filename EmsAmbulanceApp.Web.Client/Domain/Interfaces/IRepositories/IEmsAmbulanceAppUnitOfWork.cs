namespace EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;

public interface IEmsAmbulanceAppUnitOfWork : IDisposable
{
    Task<int> Complete();
    IOtpEntryRepository OtpEntry {  get; }
    IClientRepository Client {  get; }
}
