using jpfc.Data.Interfaces;
using jpfc.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data
{
    public class MetalRepository : IMetalRepository
    {
        private readonly ApplicationDbContext _context;

        public MetalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Metal>> ListMetalsAsync()
        {
            return await _context.Metal.OrderBy(e => e.Name).ToListAsync();
        }
    }
}
