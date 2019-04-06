using jpfc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IKaratRepository
    {
        Task<ICollection<Karat>> ListKaratsAsync();
    }
}
