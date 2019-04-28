using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IScheduledTaskService
    {
        Task CopyPricesToTodayAsync();
    }
}
