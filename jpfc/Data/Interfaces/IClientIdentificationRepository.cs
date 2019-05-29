using jpfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IClientIdentificationRepository
    {
        Task<bool> SaveClientIdentificationAsync(ClientIdentification clientIdentification);
    }
}
