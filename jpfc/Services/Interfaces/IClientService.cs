﻿using jpfc.Models.ClientReceiptViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IClientService
    {
        Task<(bool Success, string Error, CreateClientViewModel Model)> GetCreateClientViewModelAsync(int clientId);
        Task<(bool Success, string Error, ICollection<ClientListViewModel> Model)> GetClientListViewModelAsync();
    }
}
