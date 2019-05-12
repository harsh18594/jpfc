using jpfc.Models;
using jpfc.Models.ClientViewModels;
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

        [HttpGet]
        public async Task<IActionResult> GetClientBelongingList(int clientId)
        {
            _logger.LogInformation(GetLogDetails() + " - clientId:{@ClientId}", args: new object[] { clientId });

            var result = await _clientService.FetchClientBelongingListAsync(clientId);
            return Json(result.Model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveClientBelonging(ClientBelongingViewModel model)
        {
            _logger.LogInformation(GetLogDetails() + " - model:{@Model}", args: new object[] { model });

            // process request
            var userId = _userManager.GetUserId(User);
            var result = await _clientService.SaveClientBelongingAsync(model, userId);
            return Json(new
            {
                success = result.Success,
                error = result.Error
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientBelonging(int id)
        {
            _logger.LogInformation(GetLogDetails() + " - id:{@Id}", args: new object[] { id });

            // process request
            var result = await _clientService.DeleteClientBelongingAsync(id);
            return Json(new
            {
                success = result.Success,
                error = result.Error
            });
        }

        [HttpGet]
        public async Task<IActionResult> EditClientBelonging(int belongingId)
        {
            _logger.LogInformation(GetLogDetails() + " - belongingId:{@BelongingId}", args: new object[] { belongingId });

            // process request
            var result = await _clientService.FetchClientBelongingViewModelForEditAsync(belongingId);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                model = result.Model
            });
        }

        [HttpGet]
        public async Task<IActionResult> FetchAmountSummary(int clientId)
        {
            _logger.LogInformation(GetLogDetails() + " - clientId:{@ClientId}", args: new object[] { clientId });

            var result = await _clientService.FetchAmountSummaryViewModelAsync(clientId);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                model = result.Model
            });
        }
    }
}
