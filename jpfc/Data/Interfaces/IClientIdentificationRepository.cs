using jpfc.Models;
using jpfc.Models.ClientViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IClientIdentificationRepository
    {
        Task<bool> SaveClientIdentificationAsync(ClientIdentification clientIdentification);
        Task<ClientIdentification> FetchBaseByIdAsync(int id);
        Task<ClientIdentification> FetchFullByIdAsync(int id);
        Task<bool> DeleteClientIdentificationAsync(ClientIdentification id);
        Task<ICollection<ClientIdentificationListViewModel>> ListClientIdentificationByClientIdAsync(int clientId);
    }
}
