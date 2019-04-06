using jpfc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IMetalRepository
    {
        Task<ICollection<Metal>> ListMetalsAsync();
    }
}
