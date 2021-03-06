﻿using jpfc.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jpfc.Models.ClientViewModels;
using Microsoft.EntityFrameworkCore;
using jpfc.Models;
using jpfc.Classes;

namespace jpfc.Data
{
    public class ClientBelongingRepository : IClientBelongingRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientBelongingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ClientBelongingListViewModel>> ListClientBelongingByReceiptIdAsync(int receiptId)
        {
            return await _context.ClientBelonging
                .Where(e => e.ClientReceiptId == receiptId)
                .Select(e => new ClientBelongingListViewModel
                {
                    ClientBelongingId = e.ClientBelongingId,
                    Metal = e.MetalId.HasValue ? e.Metal.Name : e.MetalOther,
                    Karat = e.KaratId.HasValue ? e.Karat.Name : e.KaratOther,
                    ItemDescription = e.ItemDescription,
                    Weight = e.ItemWeight,
                    ItemPrice = e.ItemPrice,
                    FinalPrice = e.FinalPrice,
                    ReplacementValue = e.ReplacementValue,
                    CreatedUtc = e.CreatedUtc,
                    BusinessPaysMoney = e.TransactionAction == Constants.TransactionAction.Loan || e.TransactionAction == Constants.TransactionAction.Purchase,
                    BusinessGetsMoney = e.TransactionAction == Constants.TransactionAction.Sell,
                    TransactionAction = e.TransactionAction,
                    HstAmount = e.HstAmount
                })
                .OrderByDescending(vm => vm.CreatedUtc)
                .ToListAsync();
        }

        public async Task<ClientBelonging> FetchBaseByIdAsync(int clientBelongingId)
        {
            return await _context.ClientBelonging
                .Where(c => c.ClientBelongingId == clientBelongingId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveClientBelongingAsync(ClientBelonging clientBelonging)
        {
            if (_context.Entry(clientBelonging).State == EntityState.Detached)
            {
                _context.Add(clientBelonging);
            }
            else if (_context.Entry(clientBelonging).State == EntityState.Modified)
            {
                _context.Update(clientBelonging);
            }

            if (_context.Entry(clientBelonging).State == EntityState.Unchanged)
            {
                return true;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteBelongingAsync(ClientBelonging belonging)
        {
            _context.Remove(belonging);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
