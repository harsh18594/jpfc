using jpfc.Models.UpdatePriceViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IAdminService
    {
        (bool Success, string Error, CreatePriceViewModel Model) GetUpdatePriceViewModel();
        Task<(bool Success, string Error)> SavePriceAsync(CreatePriceViewModel model, string userId);
        Task<(bool Success, string Error, ICollection<PriceListViewModel> Model)> GetPriceListAsync();
        Task<(bool Success, string Error)> DeletePriceAsync(int id);
        Task<(bool Success, string Error)> CopyPriceAsync(int id, string userId);
        Task<(bool Success, string Error, CreatePriceViewModel Model)> GetPriceForEditAsync(int id);
    }
}
