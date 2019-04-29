using jpfc.Data.Interfaces;
using jpfc.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data
{
    public class IdentificationDocumentRepository : IIdentificationDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public IdentificationDocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<IdentificationDocument>> ListIdentificationDocumentsAsync()
        {
            return await _context.IdentificationDocument.OrderBy(e => e.Name).ToListAsync();
        }
    }
}
