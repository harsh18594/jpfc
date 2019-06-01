using jpfc.Data;
using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.ClientViewModels;
using jpfc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class ClientReceiptService : IClientReceiptService
    {
        public readonly ILogger _logger;
        public readonly IClientReceiptRepository _clientReceiptRepository;
        public readonly IClientIdentificationRepository _clientIdentificationRepository;

        public ClientReceiptService(ILogger<ClientReceiptService> logger,
            IClientReceiptRepository clientReceiptRepository,
            IClientIdentificationRepository clientIdentificationRepository)
        {
            _logger = logger;
            _clientReceiptRepository = clientReceiptRepository;
            _clientIdentificationRepository = clientIdentificationRepository;
        }

        public async Task<(bool Success, string Error, CreateClientReceiptViewModel Model)> GetCreateClientReceiptViewModelAsync(int clientId, int? receiptId)
        {
            var success = false;
            var error = "";
            var model = new CreateClientReceiptViewModel
            {
                ClientBelongingViewModel = new ClientBelongingViewModel
                {
                    BelDate = DateTime.Now
                }
            };

            try
            {
                if (clientId > 0)
                {
                    model.ClientId = clientId;
                    model.Date = DateTime.Now;

                    if (receiptId > 0)
                    {
                        var receipt = await _clientReceiptRepository.FetchBaseByIdAsync(receiptId ?? 0);
                        if (receipt != null && receipt.ClientId == clientId)
                        {
                            model.ClientReceiptId = receipt.ClientReceiptId;
                            model.ReceiptNumber = receipt.ReceiptNumber;
                            model.ClientIdentificationId = receipt.ClientIdentificationId;

                            success = true;
                        }
                        else
                        {
                            error = "Invalid Receipt Id.";
                        }
                    }
                    else
                    {
                        success = true;
                    }
                }
                else
                {
                    error = "Invalid Request";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request.";
                _logger.LogError("ClientReceiptService.GetCreateClientReceiptViewModelAsync - ex:{@Ex}", new object[] { ex });
            }

            return (success, error, model);
        }

        public async Task<(bool Success, string Error, int ReceiptId)> SaveClientReceiptAsync(CreateClientReceiptViewModel model, string userId)
        {
            var success = false;
            var error = "";
            int receiptId = 0;

            try
            {
                ClientReceipt receipt = null;
                if (model.ClientReceiptId > 0)
                {
                    receipt = await _clientReceiptRepository.FetchBaseByIdAsync(model.ClientReceiptId.Value);
                }
                if (receipt == null)
                {
                    // save reference number for new records
                    var maxReceiptId = await _clientReceiptRepository.GetMaxReceiptIdAsync();
                    var receiptNumber = $"{DateTime.Now.ToString("yyyyMMdd")}{maxReceiptId + 1}";

                    receipt = new ClientReceipt
                    {
                        CreatedUserId = userId,
                        CreatedUtc = DateTime.UtcNow,
                        // assign date and client once only when the receipt is created, do not allow modifying them
                        ClientId = model.ClientId,
                        Date = model.Date,
                        ReceiptNumber = receiptNumber

                    };
                }
                else
                {
                    receipt.AuditUserId = userId;
                    receipt.AuditUtc = DateTime.UtcNow;
                }

                // if saved id is selected, use that, else create a new one
                if (model.ClientIdentificationId.HasValue)
                {
                    receipt.ClientIdentificationId = model.ClientIdentificationId.Value;
                }
                else
                {
                    // save client id first and assign it to receipt
                    var clientIdentification = new ClientIdentification
                    {
                        ClientId = model.ClientId,
                        IdentificationDocumentId = model.IdentificationDocumentId,
                        IdentificationDocumentNumber = model.IdentificationDocumentNumber,
                        CreatedUserId = userId,
                        CreatedUtc = DateTime.UtcNow
                    };
                    await _clientIdentificationRepository.SaveClientIdentificationAsync(clientIdentification);

                    receipt.ClientIdentificationId = clientIdentification.ClientIdentificationId;
                }

                // save to database
                await _clientReceiptRepository.SaveClientReceiptAsync(receipt);
                receiptId = receipt.ClientReceiptId;
                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request.";
                _logger.LogError("ClientReceiptService.SaveClientReceiptAsync - ex:{@Ex}", new object[] { ex });
            }

            return (success, error, receiptId);
        }

        public async Task<(bool Success, string Error, ICollection<ClientReceiptViewModel> Model)> ListClientReceiptAsync(int clientId)
        {
            var success = false;
            var error = "";
            ICollection<ClientReceiptViewModel> model = new List<ClientReceiptViewModel>();

            try
            {
                model = await _clientReceiptRepository.ListByClientIdAsync(clientId);
                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request.";
                _logger.LogError("ClientReceiptService.ListClientReceiptAsync - ex:{@Ex}", new object[] { ex });
            }

            return (success, error, model);
        }
    }
}
