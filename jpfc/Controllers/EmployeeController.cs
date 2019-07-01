using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Controllers
{
    [Authorize]
    public class EmployeeController : JpfcController
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger,
            IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }
    }
}
