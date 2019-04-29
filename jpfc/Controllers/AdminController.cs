using jpfc.Models;
using jpfc.Models.ClientReceiptViewModels;
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

        public AdminController(ILogger<AdminController> logger,
            IAdminService adminService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _adminService = adminService;
            _userManager = userManager;
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
        public async Task<IActionResult> ListPrices()
        {
            _logger.LogInformation(GetLogDetails());

            var result = await _adminService.GetPriceListAsync();
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
        #endregion
    }
}
