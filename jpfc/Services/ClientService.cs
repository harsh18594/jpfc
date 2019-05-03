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

        public ClientService(ILogger<ClientService> logger,
            IClientRepository clientRepository)
        {
            _logger = logger;
            _clientRepository = clientRepository;
        }

        public async Task<(bool Success, string Error, CreateClientViewModel Model)> GetCreateClientViewModelAsync(int clientId)
        {
            var success = false;
            string error = string.Empty;
            var model = new CreateClientViewModel
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
    }
}
