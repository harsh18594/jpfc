using jpfc.Data.Interfaces;
using jpfc.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data
{
    public class KaratRepository : IKaratRepository
    {
        private readonly ApplicationDbContext _context;

        public KaratRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Karat>> ListKaratsAsync()
        {
            return await _context.Karat.OrderBy(e => e.Name).ToListAsync();
        }
    }
}
