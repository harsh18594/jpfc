using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.UpdatePriceViewModels;
using jpfc.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class AdminService : IAdminService
    {

        private readonly ILogger _logger;
        private readonly IPriceRepository _priceRepository;

        public AdminService(ILogger<AdminService> logger,
            IPriceRepository priceRepository)
        {
            _logger = logger;
            _priceRepository = priceRepository;
        }

        public (bool Success, string Error, CreatePriceViewModel Model) GetUpdatePriceViewModel()
        {
            bool success = false;
            string error = string.Empty;
            var model = new CreatePriceViewModel();

            try
            {
                model.Date = DateTime.Now;
                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("AdminService.GetUpdatePriceViewModel - Exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }

        public async Task<(bool Success, string Error)> SavePriceAsync(CreatePriceViewModel model, string userId)
        {
            bool success = false;
            string error = string.Empty;

            try
            {
                // determin if adding or editing
                Price price = null;
                if (model.PriceId > 0)
                {
                    price = await _priceRepository.FetchBasePriceByIdAsync(model.PriceId);
                }
                if (price == null)
                {
                    price = new Price
                    {
                        CreatedUserId = userId,
                        CreatedUtc = DateTime.UtcNow
                    };
                }
                else
                {
                    price.AuditUserId = userId;
                    price.AuditUtc = DateTime.UtcNow;
                }

                // save other values
                price.Date = model.Date.Value;
                price.MetalId = model.MetalId;
                price.KaratId = model.KaratId;
                price.BuyPrice = model.BuyPrice;
                price.SellPrice = model.SellPrice;
                price.LoanPrice = model.LoanPrice;
                price.LoanPricePercent = model.LoanPricePercent;

                await _priceRepository.SavePriceAsync(price);

                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("AdminService.SavePriceAsync - Exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error);
        }

        public async Task<(bool Success, string Error, ICollection<PriceListViewModel> Model)> GetPriceListAsync()
        {
            bool success = false;
            string error = string.Empty;
            ICollection<PriceListViewModel> model = new List<PriceListViewModel>();

            try
            {
                model = await _priceRepository.ListPricesAsync();
                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("AdminService.GetPriceListAsync - Exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }

        public async Task<(bool Success, string Error)> DeletePriceAsync(int id)
        {
            bool success = false;
            string error = string.Empty;

            try
            {
                if (id > 0)
                {
                    var price = await _priceRepository.FetchBasePriceByIdAsync(id);
                    if (price != null)
                    {
                        await _priceRepository.DeletePriceAsync(price);
                        success = true;
                    }
                    else
                    {
                        error = "Price information not found";
                    }
                }
                else
                {
                    error = "Invalid Id";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("AdminService.DeletePriceAsync - Exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error);
        }

        public async Task<(bool Success, string Error)> CopyPriceAsync(int id, string userId)
        {
            bool success = false;
            string error = string.Empty;

            try
            {
                if (id > 0)
                {
                    var price = await _priceRepository.FetchBasePriceByIdAsync(id);
                    if (price != null)
                    {
                        var newPrice = new Price
                        {
                            Date = DateTime.Now.Date,
                            MetalId = price.MetalId,
                            KaratId = price.KaratId,
                            BuyPrice = price.BuyPrice,
                            SellPrice = price.SellPrice,
                            LoanPrice = price.LoanPrice,
                            LoanPricePercent = price.LoanPricePercent,
                            CreatedUserId = userId,
                            CreatedUtc = DateTime.UtcNow
                        };

                        await _priceRepository.SavePriceAsync(newPrice);
                        success = true;
                    }
                    else
                    {
                        error = "Price information not found";
                    }
                }
                else
                {
                    error = "Invalid Id";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("AdminService.CopyPriceAsync - Exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error);
        }

        public async Task<(bool Success, string Error, CreatePriceViewModel Model)> GetPriceForEditAsync(int id)
        {
            bool success = false;
            string error = string.Empty;
            var model = new CreatePriceViewModel();

            try
            {
                if (id > 0)
                {
                    var price = await _priceRepository.FetchBasePriceByIdAsync(id);
                    if (price != null)
                    {
                        model.PriceId = price.PriceId;
                        model.Date = price.Date;
                        model.BuyPrice = price.BuyPrice;
                        model.SellPrice = price.SellPrice;
                        model.LoanPrice = price.LoanPrice;
                        model.LoanPricePercent = price.LoanPricePercent;
                        model.KaratId = price.KaratId;
                        model.MetalId = price.MetalId;

                        success = true;
                    }
                    else
                    {
                        error = "Price information not found";
                    }
                }
                else
                {
                    error = "Invalid Id";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("AdminService.GetPriceForEditAsync - Exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }
    }
}
