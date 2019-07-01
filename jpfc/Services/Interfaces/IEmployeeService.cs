using jpfc.Models.EmployeeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<(bool Success, string Error, EmployeeListViewModel Model)> ListAllAsync(EmployeeListViewModel model);
        Task<(bool Success, string Error)> CreateAsync(CreateEmployeeViewModel model);
        Task<(bool Success, string Error)> DeleteAsync(string id);
    }
}
