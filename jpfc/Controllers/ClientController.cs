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
        private readonly IClientIdentificationService _clientIdentificationService;
        private readonly IClientReceiptService _clientReceiptService;

        public ClientController(ILogger<AdminController> logger,
            IClientService clientService,
            UserManager<ApplicationUser> userManager,
            IClientIdentificationService clientIdentificationService,
            IClientReceiptService clientReceiptService)
        {
            _logger = logger;
            _clientService = clientService;
            _userManager = userManager;
            _clientIdentificationService = clientIdentificationService;
            _clientReceiptService = clientReceiptService;
        }

        #region Client
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [HttpGet]
        public IActionResult CreateClient()
        {
            _logger.LogInformation(GetLogDetails());
            var model = new CreateClientViewModel { Date = DateTime.Now };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(CreateClientViewModel model)
        {
            _logger.LogInformation(GetLogDetails() + " - model:{@Model}", args: new object[] { model });
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var result = await _clientService.CreateClientAsync(model, userId);
                if (result.Success)
                {
                    SetSiteMessage(MessageType.Success, DisplayFor.FullRequest, "Client saved successfully");
                    return RedirectToAction(nameof(EditClient), routeValues: new { id = result.ClientId });
                }
                else
                {
                    SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditClient(int id)
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
        public async Task<IActionResult> EditClient(CreateClientViewModel model)
        {
            _logger.LogInformation(GetLogDetails() + " - model:{@Model}", args: new object[] { model });
            // remove extra validations from viewmodel
            ModelState.Remove("IdentificationDocumentId");
            ModelState.Remove("IdentificationDocumentNumber");
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var result = await _clientService.UpdateClientAsync(model, userId);
                if (result.Success)
                {
                    SetSiteMessage(MessageType.Success, DisplayFor.FullRequest, "Client saved successfully");
                    return RedirectToAction(nameof(EditClient), routeValues: new { id = result.ClientId });
                }
                else
                {
                    SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetClientList(DateTime? startDate, DateTime? endDate)
        {
            _logger.LogInformation(GetLogDetails() + " - startDate:{@StartDate}, endDate:{@EndDate}", args: new object[] { startDate, endDate });
            var result = await _clientService.GetClientListViewModelAsync(startDate, endDate);
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
        #endregion

        #region Client Belonging
        [HttpGet]
        public async Task<IActionResult> GetClientBelongingList(int clientReceiptId)
        {
            _logger.LogInformation(GetLogDetails() + " - clientReceiptId:{@clientReceiptId}", args: new object[] { clientReceiptId });

            var result = await _clientService.FetchClientBelongingListByReceiptIdAsync(clientReceiptId);
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
        #endregion

        #region Client Identification
        [HttpPost]
        public async Task<IActionResult> SaveClientIdentification(CreateClientIdentificationViewModel model)
        {
            _logger.LogInformation(GetLogDetails() + " - model:{@Model}", new object[] { model });
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var result = await _clientIdentificationService.SaveClientIdentificationAsync(model, userId);
                return Json(new
                {
                    success = result.Success,
                    error = result.Error
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    error = "Invalid Request"
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClientIdentificationList(int clientId)
        {
            _logger.LogInformation(GetLogDetails() + " - clientId:{@ClientId}", args: new object[] { clientId });

            var result = await _clientIdentificationService.FetchClientIdentificationListAsync(clientId);
            return Json(result.Model);
        }

        [HttpGet]
        public async Task<IActionResult> EditClientIdentification(int identificationId)
        {
            _logger.LogInformation(GetLogDetails() + " - identificationId:{@IdentificationId}", args: new object[] { identificationId });

            // process request
            var result = await _clientIdentificationService.FetchCreateClientIdentificationViewModelForEditAsync(identificationId);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                model = result.Model
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientIdentification(int id)
        {
            _logger.LogInformation(GetLogDetails() + " - id:{@Id}", args: new object[] { id });

            // process request
            var result = await _clientIdentificationService.DeleteClientIdentificationAsync(id);
            return Json(new
            {
                success = result.Success,
                error = result.Error
            });
        }
        #endregion

        #region Client Receipt
        public async Task<IActionResult> Receipt(int clientId, int? receiptId)
        {
            _logger.LogInformation(GetLogDetails() + " - clientId:{@ClientId}, receiptId:{@ReceiptId}", new object[] { clientId, receiptId });
            var result = await _clientReceiptService.GetCreateClientReceiptViewModelAsync(clientId, receiptId);
            if (result.Success)
            {
                return View(result.Model);
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
            return RedirectToAction(nameof(EditClient), routeValues: new { id = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> Receipt(CreateClientReceiptViewModel model)
        {
            _logger.LogInformation(GetLogDetails() + " - model:{@Model}", new object[] { model });

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var result = await _clientReceiptService.SaveClientReceiptAsync(model, userId);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Receipt), routeValues: new { clientId = model.ClientId, receiptId = result.ReceiptId });
                }

                SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
                return RedirectToAction(nameof(EditClient), routeValues: new { id = model.ClientId });
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, "Invalid/ missing values.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetClientReceiptList(int clientId)
        {
            _logger.LogInformation(GetLogDetails() + " - clientId:{@ClientId}", new object[] { clientId });
            var result = await _clientReceiptService.ListClientReceiptAsync(clientId);
            return Json(result.Model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientReceipt(int receiptId)
        {
            _logger.LogInformation(GetLogDetails() + " - receiptId:{@ReceiptId}", new object[] { receiptId });
            var result = await _clientReceiptService.DeleteClientReceiptByIdAsync(receiptId);
            return Json(new
            {
                success = result.Success,
                error = result.Error
            });
        }

        [HttpPost]
        public async Task<IActionResult> DuplicateClientReceipt(int receiptId)
        {
            _logger.LogInformation(GetLogDetails() + " - receiptId:{@ReceiptId}", new object[] { receiptId });
            var userId = _userManager.GetUserId(User);
            var result = await _clientReceiptService.DuplicateClientReceiptByIdAsync(receiptId, userId);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                receiptId = result.ReceiptId
            });
        }

        [HttpGet]
        public async Task<IActionResult> FetchReceiptSummary(int clientReceiptId)
        {
            _logger.LogInformation(GetLogDetails() + " - clientReceiptId:{@clientReceiptId}", args: new object[] { clientReceiptId });

            var result = await _clientReceiptService.FetchReceiptSummaryAsync(clientReceiptId);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                model = result.Model
            });
        }

        [HttpPost]
        public async Task<IActionResult> ExportReceipt(int clientReceiptId)
        {
            _logger.LogInformation(GetLogDetails() + " - clientReceiptId:{@clientReceiptId}", new object[] { clientReceiptId });

            var result = await _clientReceiptService.ExportReceiptByReceiptIdAsync(clientReceiptId);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                fileBytes = result.FileBytes,
                fileName = result.FileName
            });
        }

        [HttpPost]
        public async Task<IActionResult> ExportLoanSchedule(int clientReceiptId)
        {
            _logger.LogInformation(GetLogDetails() + " - clientReceiptId:{@clientReceiptId}", new object[] { clientReceiptId });

            var result = await _clientReceiptService.ExportLoanScheduleByReceiptIdAsync(clientReceiptId);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                fileBytes = result.FileBytes,
                fileName = result.FileName
            });
        }
        #endregion
    }
}
