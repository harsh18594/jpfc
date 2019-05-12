﻿using jpfc.Models.ClientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IClientService
    {
        Task<(bool Success, string Error, CreateClientViewModel Model)> GetCreateClientViewModelAsync(int clientId);
        Task<(bool Success, string Error, ICollection<ClientListViewModel> Model)> GetClientListViewModelAsync(DateTime? startDate, DateTime? endDate);
        Task<(bool Success, string Error, int ClientId)> SaveClientAsync(CreateClientViewModel model, string userId);
        Task<(bool Success, string Error)> DeleteClientByIdAsync(int id);
        Task<(bool Success, string Error, ICollection<ClientBelongingListViewModel> Model)> FetchClientBelongingListAsync(int clientId);
        Task<(bool Success, string Error)> SaveClientBelongingAsync(ClientBelongingViewModel model, string userId);
        Task<(bool Success, string Error)> DeleteClientBelongingAsync(int id);
        Task<(bool Success, string Error, ClientBelongingViewModel Model)> FetchClientBelongingViewModelForEditAsync(int id);
        Task<(bool Success, string Error, AmountSummaryViewModel Model)> FetchAmountSummaryViewModelAsync(int clientId);
    }
}