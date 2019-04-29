using jpfc.Data.Interfaces;
using jpfc.Models.ClientReceiptViewModels;
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
            var model = new CreateClientViewModel();

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
                    var totalCount = await _clientRepository.GetTotalClientsByDateAsync(DateTime.Now.Date);
                    var refNumber = $"{DateTime.Now.ToString("yyyyMMdd")}-{totalCount + 1}";
                    model.ReferenceNumber = refNumber;
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
    }
}
