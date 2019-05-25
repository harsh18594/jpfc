using jpfc.Classes;
using jpfc.Data.Interfaces;
using jpfc.Models.UpdatePriceViewModels;
using jpfc.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IPriceRepository _priceRepository;
        private readonly ILogger _logger;

        public ServicesService(IPriceRepository priceRepository,
            ILogger<ServicesService> logger)
        {
            _logger = logger;
            _priceRepository = priceRepository;
        }

        public async Task<(bool Success, string Error, ICollection<PriceListViewModel> Model)> GetPriceListAsync(DateTime date)
        {
            bool success = false;
            string error = string.Empty;
            ICollection<PriceListViewModel> model = new List<PriceListViewModel>();

            try
            {
                var data = await _priceRepository.ListPricesAsync(date);
                var listObj = data.ToList();
                // only display 24K, 925 and 950 purity prices
                listObj.RemoveAll(e => !string.Equals(e.KaratId?.ToString(), Constants.KaratId._24K, StringComparison.InvariantCultureIgnoreCase)
                                            && !string.Equals(e.KaratId?.ToString(), Constants.KaratId._925, StringComparison.InvariantCultureIgnoreCase)
                                            && !string.Equals(e.KaratId?.ToString(), Constants.KaratId._950, StringComparison.InvariantCultureIgnoreCase));

                model = listObj;
                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("ServicesService.GetPriceListAsync - Exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }
    }
}
