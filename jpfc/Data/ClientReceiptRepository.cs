using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.ClientViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data
{
    public class ClientReceiptRepository : IClientReceiptRepository
    {
        private readonly ApplicationDbContext _context;
        public ClientReceiptRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ClientReceiptViewModel>> ListByClientIdAsync(int clientId)
        {
            return await _context.ClientReceipt
                .Where(e => e.ClientId == clientId)
                .Select(e => new ClientReceiptViewModel
                {
                    ClientId = e.ClientId,
                    ClientReceiptId = e.ClientReceiptId,
                    ReceiptNumber = e.ReceiptNumber,
                    Amount = e.ClientBelongings.Sum(b => b.FinalPrice),
                    Date = e.Date
                })
                .ToListAsync();
        }

        public async Task<ClientReceipt> FetchBaseByIdAsync(int receiptId)
        {
            return await _context.ClientReceipt
                .Where(e => e.ClientReceiptId == receiptId)
                .FirstOrDefaultAsync();
        }

        public async Task<ClientReceipt> FetchFullByIdAsync(int receiptId)
        {
            return await _context.ClientReceipt
                .Where(e => e.ClientReceiptId == receiptId)
                .Include(e => e.ClientBelongings)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveClientReceiptAsync(ClientReceipt receipt)
        {
            if (_context.Entry(receipt).State == EntityState.Detached)
            {
                _context.Add(receipt);
            }
            else if (_context.Entry(receipt).State == EntityState.Modified)
            {
                _context.Update(receipt);
            }

            if (_context.Entry(receipt).State == EntityState.Unchanged)
            {
                return true;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteClientReceiptAsync(ClientReceipt receipt)
        {
            _context.Remove(receipt);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<decimal> GetMaxReceiptIdAsync()
        {
            return await _context.ClientReceipt
                .Select(c => c.ClientReceiptId)
                .DefaultIfEmpty(0)
                .MaxAsync();
        }
    }
}
