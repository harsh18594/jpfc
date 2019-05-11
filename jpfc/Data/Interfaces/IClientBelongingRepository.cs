using jpfc.Models;
using jpfc.Models.ClientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IClientBelongingRepository
    {
        Task<ICollection<ClientBelongingListViewModel>> ListClientBelongingAsync(int clientId);
        Task<ClientBelonging> FetchBaseByIdAsync(int clientBelongingId);
        Task<bool> SaveClientBelongingAsync(ClientBelonging clientBelonging);
        Task<bool> DeleteBelongingAsync(ClientBelonging belonging);
    }
}
