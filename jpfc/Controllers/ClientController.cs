using jpfc.Models;
using jpfc.Models.ClientReceiptViewModels;
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
    public class ClientController : JpfcController
    {
        private readonly ILogger _logger;
        private readonly IClientService _clientService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientController(ILogger<AdminController> logger,
            IClientService clientService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _clientService = clientService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddClient(int id)
        {
            _logger.LogInformation(GetLogDetails() + " - id:{@Id}", args: new object[] { id });

            var result = await _clientService.GetCreateClientViewModelAsync(id);
            if (result.Success)
            {
                return View(result.Model);
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(CreateClientViewModel model)
        {
            _logger.LogInformation(GetLogDetails() + " - model:{@Model}", args: new object[] { model });
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var result = await _clientService.SaveClientAsync(model, userId);
                if (result.Success)
                {
                    SetSiteMessage(MessageType.Success, DisplayFor.FullRequest, "Client saved successfully");
                    return RedirectToAction(nameof(AddClient), routeValues: new { id = result.ClientId });
                }
                else
                {
                    SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetClientList()
        {
            _logger.LogInformation(GetLogDetails());
            var result = await _clientService.GetClientListViewModelAsync();
            return Json(result.Model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClient(int id)
        {
            _logger.LogInformation(GetLogDetails() + "", args: new object[] { id });

            var result = await _clientService.DeleteClientByIdAsync(id);
            return Json(new
            {
                success = result.Success,
                error = result.Error
            });
        }
    }
}
