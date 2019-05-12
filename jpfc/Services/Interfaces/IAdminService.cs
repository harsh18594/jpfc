using jpfc.Models.UpdatePriceViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IAdminService
    {
        (bool Success, string Error, CreatePriceViewModel Model) GetUpdatePriceViewModel();
        Task<(bool Success, string Error)> SavePriceAsync(CreatePriceViewModel model, string userId);
        Task<(bool Success, string Error, ICollection<PriceListViewModel> Model)> GetPriceListAsync(DateTime? startDate, DateTime? endDate);
        Task<(bool Success, string Error)> DeletePriceAsync(int id);
        Task<(bool Success, string Error)> CopyPriceAsync(int id, string userId);
        Task<(bool Success, string Error, CreatePriceViewModel Model)> GetPriceForEditAsync(int id);
        Task<(bool Success, string Error, decimal? Price)> FetchMetalPriceAsync(Guid metalId, Guid? karatId, DateTime? date, string clientAction);
    }
}
