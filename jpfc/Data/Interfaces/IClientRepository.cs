using jpfc.Models;
using jpfc.Models.ClientReceiptViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IClientRepository
    {
        Task<decimal> GetTotalClientsByDateAsync(DateTime date);
        Task<Client> FetchBaseByIdAsync(int clientId);
        Task<ICollection<ClientListViewModel>> ListClientsAsync();
    }
}
