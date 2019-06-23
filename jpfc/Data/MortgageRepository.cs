using jpfc.Data.Interfaces;
using jpfc.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data
{
    public class MortgageRepository : IMortgageRepository
    {
        private readonly ApplicationDbContext _context;

        public MortgageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MortgageRate> FetchMortgageRateAsync()
        {
            return await _context.MortgageRate
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveMortgageRateAsync(MortgageRate rate)
        {
            if (_context.Entry(rate).State == EntityState.Detached)
            {
                _context.Add(rate);
            }
            else if (_context.Entry(rate).State == EntityState.Modified)
            {
                _context.Update(rate);
            }

            if (_context.Entry(rate).State == EntityState.Unchanged)
            {
                return true;
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
