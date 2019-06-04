using jpfc.Classes;
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
                    BusinessPaysAmount = e.ClientBelongings
                                .Where(b => b.TransactionAction == Constants.TransactionAction.Purchase || b.TransactionAction == Constants.TransactionAction.Loan)
                                .Sum(b => b.FinalPrice)
                                .GetValueOrDefault(0),
                    BusinessGetsAmount = e.ClientBelongings
                                .Where(b => b.TransactionAction == Constants.TransactionAction.Sell)
                                .Sum(b => b.FinalPrice)
                                .GetValueOrDefault(0),
                    CreatedUtc = e.CreatedUtc,
                    PaymentAmount = e.PaymentAmount,
                    PaymentDate = e.PaymentDate
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
                .Include(e => e.Client)
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
