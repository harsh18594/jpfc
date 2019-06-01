using jpfc.Models;
using jpfc.Models.ClientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IClientReceiptService
    {
        Task<(bool Success, string Error, CreateClientReceiptViewModel Model)> GetCreateClientReceiptViewModelAsync(int clientId, int? receiptId);
        Task<(bool Success, string Error, int ReceiptId)> SaveClientReceiptAsync(CreateClientReceiptViewModel model, string userId);
        Task<(bool Success, string Error, ICollection<ClientReceiptViewModel> Model)> ListClientReceiptAsync(int clientId);
    }
}
