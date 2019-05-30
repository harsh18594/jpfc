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
    public class ClientIdentificationRepository : IClientIdentificationRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientIdentificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveClientIdentificationAsync(ClientIdentification clientIdentification)
        {
            if (_context.Entry(clientIdentification).State == EntityState.Detached)
            {
                _context.Add(clientIdentification);
            }
            else if (_context.Entry(clientIdentification).State == EntityState.Modified)
            {
                _context.Update(clientIdentification);
            }

            if (_context.Entry(clientIdentification).State == EntityState.Unchanged)
            {
                return true;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ClientIdentification> FetchBaseByIdAsync(int id)
        {
            return await _context.ClientIdentification
                .Where(c => c.ClientIdentificationId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ClientIdentification> FetchFullByIdAsync(int id)
        {
            return await _context.ClientIdentification
                .Where(c => c.ClientIdentificationId == id)
                .Include(c => c.ClientReceipts)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteClientIdentificationAsync(ClientIdentification id)
        {
            _context.Remove(id);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ICollection<ClientIdentificationListViewModel>> ListClientIdentificationByClientIdAsync(int clientId)
        {
            return await _context.ClientIdentification
                .Where(e => e.ClientId == clientId)
                .Select(e => new ClientIdentificationListViewModel
                {
                    ClientIdentificationId = e.ClientIdentificationId,
                    IdentificationType = e.IdentificationDocument.Name,
                    IdentificationNumber = e.IdentificationDocumentNumber
                })
                .OrderBy(vm => vm.IdentificationType)
                .ToListAsync();
        }
    }
}
