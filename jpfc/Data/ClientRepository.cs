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

        public async Task<decimal> GetMaxClientIdAsync()
        {
            return await _context.Client
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

        public async Task<ICollection<ClientListViewModel>> ListClientsAsync(ClientSearchViewModel model)
        {
            return await _context.Client
                .Where(e => (!model.StartDate.HasValue || e.Date >= model.StartDate)
                            && (!model.EndDate.HasValue || e.Date <= model.EndDate)
                            && (string.IsNullOrEmpty(model.FirstName) || e.FirstName.Contains(model.FirstName))
                            && (string.IsNullOrEmpty(model.LastName) || e.LastName.Contains(model.LastName)))
                .Select(e => new ClientListViewModel
                {
                    ClientId = e.ClientId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
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
            var clientReceipts = await _context.ClientReceipt.Where(cr => cr.ClientId == client.ClientId).ToListAsync();
            _context.RemoveRange(clientReceipts);

            var clientIdentification = await _context.ClientIdentification.Where(cr => cr.ClientId == client.ClientId).ToListAsync();
            _context.RemoveRange(clientIdentification);

            _context.Client.Remove(client);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
