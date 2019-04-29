using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.ClientReceiptViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTotalClientsByDateAsync(DateTime date)
        {
            return await _context.Client
                .Where(c => c.Date == date)
                .Select(c => c.ClientId)
                .SumAsync();
        }

        public async Task<Client> FetchBaseByIdAsync(int clientId)
        {
            return await _context.Client
                .Where(c => c.ClientId == clientId)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<ClientListViewModel>> ListClientsAsync()
        {
            return await _context.Client
                .Select(e => new ClientListViewModel
                {
                    ClientId = e.ClientId,
                    Name = e.Name,
                    ReferenceNumber = e.ReferenceNumber,
                    Date = e.Date
                })
                .OrderByDescending(vm => vm.CreatedUtc)
                .ToListAsync();
        }
    }
}
