using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace jpfc.Controllers
{
    public class ServicesController : JpfcController
    {
        private readonly ILogger _logger;
        private readonly IServicesService _serviceService;

        public ServicesController(ILogger<AdminController> logger,
            IServicesService serviceService)
        {
            _logger = logger;
            _serviceService = serviceService;
        }

        public IActionResult Index()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        public IActionResult BuySellGold()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListPrices(DateTime date)
        {
            _logger.LogInformation(GetLogDetails() + " - date:{@Date}", args: new object[] { date });

            var result = await _serviceService.GetPriceListAsync(date);
            return Json(result.Model);
        }

        public IActionResult CreditCards()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        public IActionResult HAInsurance()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        public IActionResult LifeInsurance()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        public IActionResult Loan()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        public IActionResult PersonalLoan()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        public IActionResult Mortgage()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        public IActionResult FinancePlanning()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }
    }
}