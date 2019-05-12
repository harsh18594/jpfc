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
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetMaxClientIdByDateAsync(DateTime date)
        {
            return await _context.Client
                .Where(c => c.Date == date)
                .Select(c => c.ClientId)
                .DefaultIfEmpty(0)
                .MaxAsync();
        }

        public async Task<Client> FetchBaseByIdAsync(int clientId)
        {
            return await _context.Client
                .Where(c => c.ClientId == clientId)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<ClientListViewModel>> ListClientsAsync(DateTime? startDate, DateTime? endDate)
        {
            return await _context.Client
                .Where(e=> (!startDate.HasValue || e.Date >= startDate) && (!endDate.HasValue || e.Date <= endDate))
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

        public async Task<bool> SaveClientAsync(Client client)
        {
            if (_context.Entry(client).State == EntityState.Detached)
            {
                _context.Add(client);
            }
            else if (_context.Entry(client).State == EntityState.Modified)
            {
                _context.Update(client);
            }

            if (_context.Entry(client).State == EntityState.Unchanged)
            {
                return true;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteClientAsync(Client client)
        {
            var clientBelongings = await _context.ClientBelonging.Where(cb => cb.ClientId == client.ClientId).ToListAsync();
            _context.RemoveRange(clientBelongings);

            _context.Client.Remove(client);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
