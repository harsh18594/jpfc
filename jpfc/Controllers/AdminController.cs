using jpfc.Models;
using jpfc.Models.ClientViewModels;
using jpfc.Models.MortgageViewModels;
using jpfc.Models.UpdatePriceViewModels;
using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Controllers
{
    [Authorize]
    public class AdminController : JpfcController
    {
        private readonly ILogger _logger;
        private readonly IAdminService _adminService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMortgageService _mortgageService;

        public AdminController(ILogger<AdminController> logger,
            IAdminService adminService,
            UserManager<ApplicationUser> userManager,
            IMortgageService mortgageService)
        {
            _logger = logger;
            _adminService = adminService;
            _userManager = userManager;
            _mortgageService = mortgageService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        #region Price
        [HttpGet]
        public IActionResult UpdatePrice()
        {
            _logger.LogInformation(GetLogDetails());

            var result = _adminService.GetUpdatePriceViewModel();
            if (result.Success)
            {
                return View(result.Model);
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SavePrice(CreatePriceViewModel model)
        {
            _logger.LogInformation(GetLogDetails() + " - model:{@Model}", args: new object[] { model });

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var result = await _adminService.SavePriceAsync(model, userId);
                return Json(new
                {
                    success = result.Success,
                    error = result.Error
                });
            }

            return Json(new
            {
                success = false,
                error = "Please review all information and try again"
            });
        }

        [HttpGet]
        public async Task<IActionResult> ListPrices(DateTime? startDate, DateTime? endDate)
        {
            _logger.LogInformation(GetLogDetails() + " - startDate:{@StartDate}, endDate:{@EndDate}", args: new object[] { startDate, endDate });

            var result = await _adminService.GetPriceListAsync(startDate, endDate);
            return Json(result.Model);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePrice(int id)
        {
            _logger.LogInformation(GetLogDetails() + " - id:{@Id}", args: new object[] { id });

            var result = await _adminService.DeletePriceAsync(id);
            return Json(new
            {
                success = result.Success,
                error = result.Error
            });
        }

        [HttpPost]
        public async Task<IActionResult> CopyPrice(int id)
        {
            _logger.LogInformation(GetLogDetails() + " - id:{@Id}", args: new object[] { id });

            var userId = _userManager.GetUserId(User);
            var result = await _adminService.CopyPriceAsync(id, userId);
            return Json(new
            {
                success = result.Success,
                error = result.Error
            });
        }

        [HttpGet]
        public async Task<IActionResult> EditPrice(int id)
        {
            _logger.LogInformation(GetLogDetails() + " - id:{@Id}", args: new object[] { id });

            var userId = _userManager.GetUserId(User);
            var result = await _adminService.GetPriceForEditAsync(id);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                model = result.Model
            });
        }

        [HttpGet]
        public async Task<IActionResult> FetchMetalPrice(Guid metalId, Guid? karatId, DateTime? date, string transactionAction)
        {
            _logger.LogInformation(GetLogDetails() + " - metalId:{@MetalId}, karatId:{@KaratId}, date:{@Date}, transactionAction:{@TransactionAction}",
                args: new object[] { metalId, karatId, date, transactionAction });

            var userId = _userManager.GetUserId(User);
            var result = await _adminService.FetchMetalPriceAsync(metalId, karatId, date, transactionAction);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                price = result.Price
            });
        }
        #endregion

        #region Mortgage Rate
        public async Task<IActionResult> MortgageRate()
        {
            _logger.LogInformation(GetLogDetails());
            var result = await _mortgageService.GetCreateMortgageRateViewModelAsync();
            if (result.Success)
            {
                return View(result.Model);
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MortgageRate(CreateMortgageRateViewModel model)
        {
            _logger.LogInformation(GetLogDetails());

            var userId = _userManager.GetUserId(User);
            var result = await _mortgageService.SaveMortgageRateAsync(model, userId);
            if (result.Success)
            {
                SetSiteMessage(MessageType.Success, DisplayFor.FullRequest, "Mortgage rate has been saved successfully.");
            }
            else
            {
                SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
