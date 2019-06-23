using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.MortgageViewModels;
using jpfc.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class MortgageService : IMortgageService
    {
        private readonly ILogger _logger;
        private readonly IMortgageRepository _mortgageRepository;

        public MortgageService(ILogger<MortgageService> logger,
            IMortgageRepository mortgageRepository)
        {
            _logger = logger;
            _mortgageRepository = mortgageRepository;
        }

        public async Task<(bool Success, string Error, CreateMortgageRateViewModel Model)> GetCreateMortgageRateViewModelAsync()
        {
            var success = false;
            var error = "";
            CreateMortgageRateViewModel model = new CreateMortgageRateViewModel();

            try
            {
                var rate = await _mortgageRepository.FetchMortgageRateAsync();
                if (rate != null)
                {
                    model.Rate = rate.Rate;
                }

                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("MortgageService.GetCreateMortgageRateViewModelAsync - exception:{@Ex}", new object[] { ex });
            }

            return (success, error, model);
        }

        public async Task<(bool Success, string Error)> SaveMortgageRateAsync(CreateMortgageRateViewModel model, string userId)
        {
            var success = false;
            var error = "";

            try
            {
                var rate = await _mortgageRepository.FetchMortgageRateAsync();
                if (rate == null)
                {
                    rate = new MortgageRate
                    {
                        CreatedUtc = DateTime.UtcNow,
                        CreatedUserId = userId
                    };
                }
                else
                {
                    rate.AuditUserId = userId;
                    rate.AuditUtc = DateTime.UtcNow;
                }

                rate.Rate = model.Rate;

                await _mortgageRepository.SaveMortgageRateAsync(rate);
                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("MortgageService.SaveMortgageRateAsync - exception:{@Ex}", new object[] { ex });
            }

            return (success, error);
        }

        public async Task<(bool Success, string Error, ViewMortgageRateViewModel Model)> FetchMortgageRateAsync()
        {
            var success = false;
            var error = "";
            ViewMortgageRateViewModel model = new ViewMortgageRateViewModel();

            try
            {
                var rate = await _mortgageRepository.FetchMortgageRateAsync();
                if (rate != null)
                {
                    model.Rate = rate.Rate;
                }

                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("MortgageService.FetchMortgageRateAsync - exception:{@Ex}", new object[] { ex });
            }

            return (success, error, model);
        }
    }
}
