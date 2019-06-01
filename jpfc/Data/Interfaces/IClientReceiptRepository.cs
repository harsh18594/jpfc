using jpfc.Models;
using jpfc.Models.ClientViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IClientReceiptRepository
    {
        Task<ICollection<ClientReceiptViewModel>> ListByClientIdAsync(int clientId);
        Task<ClientReceipt> FetchBaseByIdAsync(int receiptId);
        Task<ClientReceipt> FetchFullByIdAsync(int receiptId);
        Task<bool> SaveClientReceiptAsync(ClientReceipt receipt);
        Task<bool> DeleteClientReceiptAsync(ClientReceipt receipt);
        Task<decimal> GetMaxReceiptIdAsync();
    }
}
