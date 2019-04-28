using jpfc.Models.DropdownViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IKaratService
    {
        Task<ICollection<KaratDropdownViewModel>> ListKaratByMetalIdAsync(Guid? metalId);
    }
}
