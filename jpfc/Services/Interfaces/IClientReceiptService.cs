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
        Task<(bool Success, string Error)> DeleteClientReceiptByIdAsync(int receiptId);
        Task<(bool Success, string Error, int ReceiptId)> DuplicateClientReceiptByIdAsync(int receiptId, string userId);
        Task<(bool Success, string Error, AmountSummaryViewModel Model)> FetchReceiptSummaryAsync(int clientReceiptId);
        Task<(bool Success, string Error, byte[] FileBytes, string FileName)> ExportReceiptByReceiptIdAsync(int clientReceiptId);
        Task<(bool Success, string Error, byte[] FileBytes, string FileName)> ExportLoanScheduleByReceiptIdAsync(int clientReceiptId);
        Task<(bool Success, string Error, byte[] FileBytes, string FileName)> ExportPaymentReceiptByReceiptIdAsync(int clientReceiptId);
    }
}
