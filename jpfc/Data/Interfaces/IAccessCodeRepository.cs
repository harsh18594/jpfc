using jpfc.Models;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IAccessCodeRepository
    {
        Task<AccessCode> GetAccessCodeAsync();
        Task<bool> SaveAccessCodeAsync(AccessCode code);
    }
}
