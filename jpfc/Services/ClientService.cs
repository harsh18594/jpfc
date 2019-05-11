using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.ClientViewModels;
using jpfc.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class ClientService : IClientService
    {
        private readonly ILogger _logger;
        private readonly IClientRepository _clientRepository;
        private readonly IClientBelongingRepository _clientBelongingRepository;

        public ClientService(ILogger<ClientService> logger,
            IClientRepository clientRepository,
            IClientBelongingRepository clientBelongingRepository)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _clientBelongingRepository = clientBelongingRepository;
        }

        #region Client
        public async Task<(bool Success, string Error, CreateClientViewModel Model)> GetCreateClientViewModelAsync(int clientId)
        {
            var success = false;
            string error = string.Empty;
            var model = new CreateClientViewModel
            {
                ClientBelongingViewModel = new ClientBelongingViewModel
                {
                    BelDate = DateTime.Now,
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
                        model.Name = client.Name;
                        model.EmailAddress = client.EmailAddress;
                        model.ContactNumber = client.ContactNumber;
                        model.Address = client.Address;
                        model.IdentificationDocumentId = client.IdentificationDocumentId;
                        model.IdentificationDocumentNumber = client.IdentificationDocumentNumber;

                        success = true;
                    }
                    else
                    {
                        error = "Unable to locate client information. Please try again.";
                    }
                }
                else
                {

                    model.ReferenceNumber = "";
                    model.Date = DateTime.Now.Date;

                    success = true;
                }
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.GetCreateClientViewModelAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }

        public async Task<(bool Success, string Error, ICollection<ClientListViewModel> Model)> GetClientListViewModelAsync()
        {
            var success = false;
            string error = string.Empty;
            ICollection<ClientListViewModel> model = new List<ClientListViewModel>();

            try
            {
                model = await _clientRepository.ListClientsAsync();
                success = true;
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.GetClientListViewModelAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }

        public async Task<(bool Success, string Error, int ClientId)> SaveClientAsync(CreateClientViewModel model, string userId)
        {
            var success = false;
            var error = string.Empty;
            var clientId = 0;

            try
            {
                Client client = null;
                if (model.ClientId > 0)
                {
                    client = await _clientRepository.FetchBaseByIdAsync(model.ClientId.Value);
                }
                if (client == null)
                {
                    client = new Client
                    {
                        CreatedUserId = userId,
                        CreatedUtc = DateTime.UtcNow
                    };

                    // save reference number for new records
                    var maxClientId = await _clientRepository.GetMaxClientIdByDateAsync(DateTime.Now.Date);
                    var refNumber = $"{DateTime.Now.ToString("yyyyMMdd")}-{maxClientId + 1}";
                    client.ReferenceNumber = refNumber;
                }
                else
                {
                    client.AuditUserId = userId;
                    client.AuditUtc = DateTime.UtcNow;
                }

                // save other values
                client.Address = model.Address;
                client.Date = model.Date;
                client.IdentificationDocumentId = model.IdentificationDocumentId;
                client.IdentificationDocumentNumber = model.IdentificationDocumentNumber;
                client.Name = model.Name;
                client.ContactNumber = model.ContactNumber;
                client.EmailAddress = model.EmailAddress;

                await _clientRepository.SaveClientAsync(client);

                clientId = client.ClientId;
                success = true;
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.SaveClientAsync - exception:{@Ex}", args: new object[] { ex });
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

        public async Task<(bool Success, string Error, ICollection<ClientBelongingListViewModel> Model)> FetchClientBelongingListAsync(int clientId)
        {
            var success = false;
            string error = string.Empty;
            ICollection<ClientBelongingListViewModel> model = new List<ClientBelongingListViewModel>();

            try
            {
                model = await _clientBelongingRepository.ListClientBelongingAsync(clientId);
                success = true;
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.FetchClientBelongingListAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }

        public async Task<(bool Success, string Error)> SaveClientBelongingAsync(ClientBelongingViewModel model, string userId)
        {
            var success = false;
            var error = string.Empty;

            try
            {
                if (model.ClientId > 0)
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
                    clientBelonging.ClientId = model.ClientId;
                    clientBelonging.Date = model.BelDate;
                    clientBelonging.ClientAction = model.ClientAction;
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
                        model.BelDate = belonging.Date;
                        model.ClientAction = belonging.ClientAction;
                        model.MetalId = belonging.MetalId;
                        model.MetalOther = belonging.MetalOther;
                        model.KaratId = belonging.KaratId;
                        model.KaratOther = belonging.KaratOther;
                        model.Weight = belonging.ItemWeight;
                        model.ItemPrice = belonging.ItemPrice;
                        model.FinalPrice = belonging.FinalPrice;

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
