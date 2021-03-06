﻿using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.UpdatePriceViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data
{
    public class PriceRepository : IPriceRepository
    {
        private readonly ApplicationDbContext _context;

        public PriceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Price> FetchBasePriceByIdAsync(int id)
        {
            return await _context.Price.Where(e => e.PriceId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> SavePriceAsync(Price price)
        {
            if (_context.Entry(price).State == EntityState.Detached)
            {
                _context.Add(price);
            }
            else if (_context.Entry(price).State == EntityState.Modified)
            {
                _context.Update(price);
            }

            if (_context.Entry(price).State == EntityState.Unchanged)
            {
                return true;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePriceAsync(Price price)
        {
            _context.Remove(price);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ICollection<PriceListViewModel>> ListPricesAsync(DateTime? date = null)
        {
            return await _context.Price
                .Where(e => !date.HasValue || e.Date == date)
                .Select(e => new PriceListViewModel
                {
                    PriceId = e.PriceId,
                    Date = e.Date,
                    BuyPrice = e.BuyPrice,
                    SellPrice = e.SellPrice,
                    LoanPrice = e.LoanPrice,
                    Metal = e.Metal.Name,
                    Karat = e.Karat.Name,
                    KaratId = e.KaratId,
                    CreatedUtc = e.CreatedUtc,
                    PerOunce = e.PerOunce
                })
                .OrderBy(e => e.Metal)
                .ThenBy(e => e.Karat)
                .ThenByDescending(e => e.CreatedUtc)
                .ToListAsync();
        }

        public async Task<ICollection<PriceListViewModel>> ListPricesByDateRangeAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _context.Price
                .Where(e => (!startDate.HasValue || e.Date >= startDate) && (!endDate.HasValue || e.Date <= endDate))
                .Select(e => new PriceListViewModel
                {
                    PriceId = e.PriceId,
                    Date = e.Date,
                    BuyPrice = e.BuyPrice,
                    SellPrice = e.SellPrice,
                    LoanPrice = e.LoanPrice,
                    Metal = e.Metal.Name,
                    Karat = e.Karat.Name,
                    PerOunce = e.PerOunce,
                    CreatedUtc = e.CreatedUtc
                })
                .OrderBy(e => e.Metal)
                .ThenBy(e => e.Karat)
                .ThenByDescending(e => e.CreatedUtc)
                .ToListAsync();
        }

        public async Task<ICollection<Price>> ListBasePricesByDateAsync(DateTime date)
        {
            return await _context.Price
                .Where(e => e.Date == date)
                .OrderBy(e => e.Metal)
                .ThenBy(e => e.Karat)
                .ThenByDescending(e => e.CreatedUtc)
                .ToListAsync();
        }

        public async Task<Price> FetchPriceByMetalIdKaratIdAsync(Guid metalId, Guid? karatId, DateTime? date = null)
        {
            return await _context.Price
                .Where(e => e.MetalId == metalId && e.KaratId == karatId && (!date.HasValue || e.Date == date))
                .OrderByDescending(e => e.Date)
                .FirstOrDefaultAsync();
        }
    }
}
