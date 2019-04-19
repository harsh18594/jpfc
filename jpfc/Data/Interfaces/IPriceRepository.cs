﻿using jpfc.Models;
using jpfc.Models.UpdatePriceViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IPriceRepository
    {
        Task<Price> FetchBasePriceByIdAsync(int id);
        Task<bool> SavePriceAsync(Price price);
        Task<bool> DeletePriceAsync(Price price);
        Task<ICollection<PriceListViewModel>> ListPricesAsync(DateTime? date = null);
    }
}
