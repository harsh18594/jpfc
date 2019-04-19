using jpfc.Models.UpdatePriceViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IServicesService
    {
        Task<(bool Success, string Error, ICollection<PriceListViewModel> Model)> GetPriceListAsync(DateTime date);
    }
}
