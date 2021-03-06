﻿using jpfc.Models;
using jpfc.Models.ClientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IClientRepository
    {
        Task<decimal> GetMaxClientIdAsync();
        Task<Client> FetchBaseByIdAsync(int clientId);
        Task<ICollection<ClientListViewModel>> ListClientsAsync(ClientSearchViewModel model);
        Task<bool> SaveClientAsync(Client client);
        Task<bool> DeleteClientAsync(Client client);
    }
}
