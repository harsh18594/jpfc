using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace jpfc.Controllers
{
    // do not apply attribute route on controller level
    public class ServicesController : JpfcController
    {
        private readonly ILogger _logger;
        private readonly IServicesService _serviceService;
        private readonly IMortgageService _mortgageService;

        public ServicesController(ILogger<AdminController> logger,
            IServicesService serviceService,
            IMortgageService mortgageService)
        {
            _logger = logger;
            _serviceService = serviceService;
            _mortgageService = mortgageService;
        }

        [Route("services")]
        public IActionResult Index()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [Route("services/trade-precious-metal")]
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

        [Route("services/credit-cards")]
        public IActionResult CreditCards()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [Route("services/home-auto-insurance")]
        public IActionResult HAInsurance()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [Route("services/life-insurance")]
        public IActionResult LifeInsurance()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [Route("services/pawn-precious-thing")]
        public IActionResult Loan()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [Route("services/personal-loan")]
        public IActionResult PersonalLoan()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [Route("services/mortgage")]
        public async Task<IActionResult> Mortgage()
        {
            _logger.LogInformation(GetLogDetails());
            var result = await _mortgageService.FetchMortgageRateAsync();
            return View(result.Model);
        }

        [Route("services/finance-planning")]
        public IActionResult FinancePlanning()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [Route("services/car-loan")]
        public IActionResult CarLoan()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }
    }
}