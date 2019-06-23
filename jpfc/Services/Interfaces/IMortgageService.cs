using jpfc.Models.MortgageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IMortgageService
    {
        Task<(bool Success, string Error, CreateMortgageRateViewModel Model)> GetCreateMortgageRateViewModelAsync();
        Task<(bool Success, string Error)> SaveMortgageRateAsync(CreateMortgageRateViewModel model, string userId);
        Task<(bool Success, string Error, ViewMortgageRateViewModel Model)> FetchMortgageRateAsync();
    }
}
