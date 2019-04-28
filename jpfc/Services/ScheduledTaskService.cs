using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class ScheduledTaskService : IScheduledTaskService
    {
        private readonly ILogger _logger;
        private readonly IPriceRepository _priceRepository;

        public ScheduledTaskService(ILogger<ScheduledTaskService> logger,
            IPriceRepository priceRepository)
        {
            _logger = logger;
            _priceRepository = priceRepository;
        }

        public async Task CopyPricesToTodayAsync()
        {
            try
            {
                var previousDate = DateTime.Now.AddDays(-1).Date;
                var previousDayPrices = await _priceRepository.ListBasePricesByDateAsync(previousDate);
                if (previousDayPrices.Any())
                {
                    foreach (var price in previousDayPrices)
                    {
                        // avoid copying, if the price already exist
                        var possibleExistingPrice = await _priceRepository.FetchLatestPriceByMetalIdKaratIdAsync(price.MetalId, price.KaratId);
                        if (possibleExistingPrice == null)
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
                                CreatedUserId = null,
                                CreatedUtc = DateTime.UtcNow
                            };

                            await _priceRepository.SavePriceAsync(newPrice);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ScheduledTaskService.CopyPricesToTodayAsync - exception:{@Ex}", args: new object[] { ex });
            }
        }
    }
}
