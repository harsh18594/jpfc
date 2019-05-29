using jpfc.Data.Interfaces;
using jpfc.Models;
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
    }
}
