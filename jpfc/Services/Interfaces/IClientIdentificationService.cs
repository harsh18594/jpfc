using jpfc.Models.ClientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IClientIdentificationService
    {
        Task<(bool Success, string Error)> SaveClientIdentificationAsync(CreateClientIdentificationViewModel model, string userId);
        Task<(bool Success, string Error, ICollection<ClientIdentificationListViewModel> Model)> FetchClientIdentificationListAsync(int clientId);
        Task<(bool Success, string Error, CreateClientIdentificationViewModel Model)> FetchCreateClientIdentificationViewModelForEditAsync(int id);
        Task<(bool Success, string Error)> DeleteClientIdentificationAsync(int id);
    }
}
