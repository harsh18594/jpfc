using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.ClientViewModels;
using jpfc.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class ClientService : IClientService
    {
        private readonly ILogger _logger;
        private readonly IClientRepository _clientRepository;
        private readonly IClientBelongingRepository _clientBelongingRepository;
        private readonly IClientIdentificationRepository _clientIdentificationRepository;
        private readonly IClientReceiptRepository _clientReceiptRepository;

        public ClientService(ILogger<ClientService> logger,
            IClientRepository clientRepository,
            IClientBelongingRepository clientBelongingRepository,
            IClientIdentificationRepository clientIdentificationRepository,
            IClientReceiptRepository clientReceiptRepository)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _clientBelongingRepository = clientBelongingRepository;
            _clientIdentificationRepository = clientIdentificationRepository;
            _clientReceiptRepository = clientReceiptRepository;
        }

        #region Client
        public async Task<(bool Success, string Error, ICollection<ClientListViewModel> Model)> GetClientListViewModelAsync(DateTime? startDate, DateTime? endDate)
        {
            var success = false;
            string error = string.Empty;
            ICollection<ClientListViewModel> model = new List<ClientListViewModel>();

            try
            {
                model = await _clientRepository.ListClientsAsync(startDate, endDate);
                success = true;
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.GetClientListViewModelAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }

        public async Task<(bool Success, string Error, CreateClientViewModel Model)> GetCreateClientViewModelAsync(int clientId)
        {
            var success = false;
            string error = string.Empty;
            var model = new CreateClientViewModel
            {
                CreateClientIdentificationViewModel = new CreateClientIdentificationViewModel
                {
                    ClientId = clientId
                }
            };

            try
            {
                if (clientId > 0)
                {
                    var client = await _clientRepository.FetchBaseByIdAsync(clientId);
                    if (client != null)
                    {
                        model.ClientId = client.ClientId;
                        model.Date = client.Date;
                        model.ReferenceNumber = client.ReferenceNumber;
                        model.FirstName = client.FirstName;
                        model.LastName = client.LastName;
                        model.EmailAddress = client.EmailAddress;
                        model.ContactNumber = client.ContactNumber;
                        model.Address = client.Address;

                        success = true;
                    }
                    else
                    {
                        error = "Unable to locate client information. Please try again.";
                    }
                }
                else
                {
                    error = "Invalid Request";
                }
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.GetCreateClientViewModelAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }

        public async Task<(bool Success, string Error, int ClientId)> CreateClientAsync(CreateClientViewModel model, string userId)
        {
            var success = false;
            var error = string.Empty;
            var clientId = 0;

            try
            {
                var client = new Client
                {
                    CreatedUserId = userId,
                    CreatedUtc = DateTime.UtcNow
                };

                // save reference number for new records
                var maxClientId = await _clientRepository.GetMaxClientIdAsync();
                var refNumber = $"CL{DateTime.Now.ToString("yyyyMMdd")}{maxClientId + 1}";
                client.ReferenceNumber = refNumber;

                // save other values
                client.Address = model.Address;
                client.Date = model.Date;
                client.FirstName = model.FirstName;
                client.LastName = model.LastName;
                client.ContactNumber = model.ContactNumber;
                client.EmailAddress = model.EmailAddress;

                await _clientRepository.SaveClientAsync(client);

                // save client identification information once client is saved
                var clientIdentification = new ClientIdentification
                {
                    ClientId = client.ClientId,
                    IdentificationDocumentId = model.IdentificationDocumentId,
                    IdentificationDocumentNumber = model.IdentificationDocumentNumber,
                    CreatedUserId = userId,
                    CreatedUtc = DateTime.UtcNow
                };
                await _clientIdentificationRepository.SaveClientIdentificationAsync(clientIdentification);

                clientId = client.ClientId;
                success = true;
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.CreateClientAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, ClientId: clientId);
        }

        public async Task<(bool Success, string Error, int ClientId)> UpdateClientAsync(CreateClientViewModel model, string userId)
        {
            var success = false;
            var error = string.Empty;
            var clientId = 0;

            try
            {
                if (model.ClientId > 0)
                {
                    var client = await _clientRepository.FetchBaseByIdAsync(model.ClientId.Value);
                    client.AuditUserId = userId;
                    client.AuditUtc = DateTime.UtcNow;
                    client.Address = model.Address;
                    client.Date = model.Date;
                    client.FirstName = model.FirstName;
                    client.LastName = model.LastName;
                    client.ContactNumber = model.ContactNumber;
                    client.EmailAddress = model.EmailAddress;

                    await _clientRepository.SaveClientAsync(client);

                    clientId = client.ClientId;
                    success = true;
                }
                else
                {
                    error = "Invalid Request";
                }
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.UpdateClientAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, ClientId: clientId);
        }

        public async Task<(bool Success, string Error)> DeleteClientByIdAsync(int id)
        {
            var success = false;
            var error = string.Empty;

            try
            {
                if (id > 0)
                {
                    var client = await _clientRepository.FetchBaseByIdAsync(id);
                    if (client != null)
                    {
                        await _clientRepository.DeleteClientAsync(client);
                        success = true;
                    }
                    else
                    {
                        error = "Unable to locate client information.";
                    }
                }
                else
                {
                    error = "Invalid Id.";
                }
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.DeleteClientByIdAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error);
        }
        #endregion

        #region Client Belonging

        public async Task<(bool Success, string Error, ICollection<ClientBelongingListViewModel> Model)> FetchClientBelongingListByReceiptIdAsync(int receiptId)
        {
            var success = false;
            string error = string.Empty;
            ICollection<ClientBelongingListViewModel> model = new List<ClientBelongingListViewModel>();

            try
            {
                model = await _clientBelongingRepository.ListClientBelongingByReceiptIdAsync(receiptId);
                success = true;
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.FetchClientBelongingListByReceiptIdAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }

        public async Task<(bool Success, string Error)> SaveClientBelongingAsync(ClientBelongingViewModel model, string userId)
        {
            var success = false;
            var error = string.Empty;

            try
            {
                if (model.ClientReceiptId > 0)
                {
                    ClientBelonging clientBelonging = null;
                    if (model.ClientBelongingId > 0)
                    {
                        clientBelonging = await _clientBelongingRepository.FetchBaseByIdAsync(model.ClientBelongingId.Value);
                    }
                    if (clientBelonging == null)
                    {
                        clientBelonging = new ClientBelonging
                        {
                            CreatedUserId = userId,
                            CreatedUtc = DateTime.UtcNow
                        };
                    }
                    else
                    {
                        clientBelonging.AuditUserId = userId;
                        clientBelonging.AuditUtc = DateTime.UtcNow;
                    }

                    // save other values
                    clientBelonging.ClientReceiptId = model.ClientReceiptId;
                    clientBelonging.TransactionAction = model.TransactionAction;
                    if (model.MetalId.HasValue && model.MetalId != Guid.Empty)
                    {
                        clientBelonging.MetalId = model.MetalId;
                        clientBelonging.MetalOther = null;
                    }
                    else
                    {
                        clientBelonging.MetalId = null;
                        clientBelonging.MetalOther = model.MetalOther;
                    }

                    if (model.KaratId.HasValue && model.KaratId != Guid.Empty)
                    {
                        clientBelonging.KaratId = model.KaratId;
                        clientBelonging.KaratOther = null;
                    }
                    else
                    {
                        clientBelonging.KaratId = null;
                        clientBelonging.KaratOther = model.KaratOther;
                    }
                    clientBelonging.ItemWeight = model.Weight;
                    clientBelonging.ItemPrice = model.ItemPrice;
                    clientBelonging.FinalPrice = model.FinalPrice;
                    clientBelonging.ReplacementValue = model.ReplacementValue;

                    await _clientBelongingRepository.SaveClientBelongingAsync(clientBelonging);
                    success = true;
                }
                else
                {
                    error = "Invalid request";
                }
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.SaveClientBelongingAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error);
        }

        public async Task<(bool Success, string Error)> DeleteClientBelongingAsync(int id)
        {
            var success = false;
            var error = "";

            try
            {
                if (id > 0)
                {
                    var belonging = await _clientBelongingRepository.FetchBaseByIdAsync(id);
                    if (belonging != null)
                    {
                        await _clientBelongingRepository.DeleteBelongingAsync(belonging);
                        success = true;
                    }
                    else
                    {
                        error = "Unable to locate data";
                    }
                }
                else
                {
                    error = "Invalid request";
                }
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.DeleteClientBelongingAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error);
        }

        public async Task<(bool Success, string Error, ClientBelongingViewModel Model)> FetchClientBelongingViewModelForEditAsync(int id)
        {
            var success = false;
            var error = "";
            var model = new ClientBelongingViewModel();

            try
            {
                if (id > 0)
                {
                    var belonging = await _clientBelongingRepository.FetchBaseByIdAsync(id);
                    if (belonging != null)
                    {
                        model.ClientBelongingId = belonging.ClientBelongingId;
                        model.TransactionAction = belonging.TransactionAction;
                        model.MetalId = belonging.MetalId;
                        model.MetalOther = belonging.MetalOther;
                        model.KaratId = belonging.KaratId;
                        model.KaratOther = belonging.KaratOther;
                        model.Weight = belonging.ItemWeight;
                        model.ItemPrice = belonging.ItemPrice;
                        model.FinalPrice = belonging.FinalPrice;
                        model.ReplacementValue = belonging.ReplacementValue;
                        model.ClientReceiptId = belonging.ClientReceiptId;

                        success = true;
                    }
                    else
                    {
                        error = "Unable to locate data";
                    }
                }
                else
                {
                    error = "Invalid request";
                }
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.FetchClientBelongingViewModelForEditAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }
        #endregion
    }
}
