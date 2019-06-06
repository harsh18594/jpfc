using jpfc.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jpfc.Data.Interfaces;
using jpfc.Models.ClientViewModels;
using jpfc.Models;
using jpfc.Classes;

namespace jpfc.Services
{
    public class ClientIdentificationService : IClientIdentificationService
    {
        private readonly ILogger _logger;
        private readonly IClientIdentificationRepository _clientIdentificationRepository;

        public ClientIdentificationService(ILogger<ClientIdentificationService> logger,
            IClientIdentificationRepository clientIdentificationRepository)
        {
            _logger = logger;
            _clientIdentificationRepository = clientIdentificationRepository;
        }

        public async Task<(bool Success, string Error)> SaveClientIdentificationAsync(CreateClientIdentificationViewModel model, string userId)
        {
            var success = false;
            var error = "";

            try
            {
                ClientIdentification id = null;
                if (model.ClientIdentificationId > 0)
                {
                    id = await _clientIdentificationRepository.FetchBaseByIdAsync(model.ClientIdentificationId);
                }
                if (id == null)
                {
                    id = new ClientIdentification
                    {
                        CreatedUtc = DateTime.UtcNow,
                        CreatedUserId = userId
                    };
                }
                else
                {
                    id.AuditUserId = userId;
                    id.AuditUtc = DateTime.UtcNow;
                }

                // save other values
                id.ClientId = model.ClientId;
                id.IdentificationDocumentId = model.IdentificationDocumentId;
                // encrypt id info
                var idNumberEncryptionResult = Encryption.Encrypt(model.IdentificationDocumentNumber);
                id.IdentificationDocumentNumberEncrypted = idNumberEncryptionResult.EncryptedString;
                id.IdentificaitonDocumentNumberUniqueKey = idNumberEncryptionResult.UniqueKey;

                await _clientIdentificationRepository.SaveClientIdentificationAsync(id);
                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request.";
                _logger.LogError("ClientIdentificationService.SaveClientIdentificationAsync - exception:{@Ex}", new object[] { ex });
            }

            return (success, error);
        }

        public async Task<(bool Success, string Error, ICollection<ClientIdentificationListViewModel> Model)> FetchClientIdentificationListAsync(int clientId)
        {
            var success = false;
            string error = string.Empty;
            ICollection<ClientIdentificationListViewModel> model = new List<ClientIdentificationListViewModel>();

            try
            {
                model = await _clientIdentificationRepository.ListClientIdentificationByClientIdAsync(clientId);
                if (model?.Any() == true)
                {
                    foreach (var item in model)
                    {
                        item.IdentificationNumber = Encryption.Decrypt(item.IdentificationNumberEncryptedString, item.IdentificationNumberUniqueKey);
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientIdentificationService.FetchClientIdentificationListAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }

        public async Task<(bool Success, string Error, CreateClientIdentificationViewModel Model)> FetchCreateClientIdentificationViewModelForEditAsync(int id)
        {
            var success = false;
            var error = "";
            var model = new CreateClientIdentificationViewModel();

            try
            {
                if (id > 0)
                {
                    var identification = await _clientIdentificationRepository.FetchBaseByIdAsync(id);
                    if (identification != null)
                    {
                        model.ClientId = identification.ClientId;
                        model.ClientIdentificationId = identification.ClientIdentificationId;
                        model.IdentificationDocumentId = identification.IdentificationDocumentId;
                        model.IdentificationDocumentNumber = Encryption.Decrypt(identification.IdentificationDocumentNumberEncrypted, identification.IdentificaitonDocumentNumberUniqueKey);
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
                _logger.LogError("ClientIdentificationService.FetchCreateClientIdentificationViewModelForEditAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }

        public async Task<(bool Success, string Error)> DeleteClientIdentificationAsync(int id)
        {
            var success = false;
            var error = "";

            try
            {
                if (id > 0)
                {
                    var identification = await _clientIdentificationRepository.FetchFullByIdAsync(id);
                    if (identification != null)
                    {
                        if (identification.ClientReceipts?.Any() == false)
                        {
                            await _clientIdentificationRepository.DeleteClientIdentificationAsync(identification);
                            success = true;
                        }
                        else
                        {
                            error = $"Unable to delete. Record is used for {identification.ClientReceipts.Count} receipt(s).";
                        }
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
                _logger.LogError("ClientIdentificationService.DeleteClientIdentificationAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error);
        }
    }
}
