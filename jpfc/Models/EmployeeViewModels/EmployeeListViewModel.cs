using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.EmployeeViewModels
{
    public class EmployeeListViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>(); 
    }
}
