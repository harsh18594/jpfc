using jpfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IMortgageRepository
    {
        Task<MortgageRate> FetchMortgageRateAsync();
        Task<bool> SaveMortgageRateAsync(MortgageRate rate);
    }
}
