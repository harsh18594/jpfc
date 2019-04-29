using jpfc.Models;
using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Controllers
{
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
        public async Task<IActionResult> AddClient(int clientId)
        {
            _logger.LogInformation(GetLogDetails() + " - clientId:{@ClientId}", args: new object[] { clientId });

            var result = await _clientService.GetCreateClientViewModelAsync(clientId);
            if (result.Success)
            {
                return View(result.Model);
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetClientList()
        {
            _logger.LogInformation(GetLogDetails());
            var result = await _clientService.GetClientListViewModelAsync();
            return Json(result.Model);
        }
    }
}
