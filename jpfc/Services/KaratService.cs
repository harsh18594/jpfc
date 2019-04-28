using jpfc.Data.Interfaces;
using jpfc.Models.DropdownViewModels;
using jpfc.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class KaratService : IKaratService
    {
        private readonly ILogger _logger;
        private readonly IKaratRepository _karatRepository;

        public KaratService(ILogger<KaratService> logger,
            IKaratRepository karatRepository)
        {
            _logger = logger;
            _karatRepository = karatRepository;
        }

        public async Task<ICollection<KaratDropdownViewModel>> ListKaratByMetalIdAsync(Guid? metalId)
        {
            var model = new List<KaratDropdownViewModel>();

            try
            {
                var karats = await _karatRepository.ListKaratsAsync(metalId);
                if (karats.Any())
                {
                    foreach (var karat in karats)
                    {
                        var vm = new KaratDropdownViewModel
                        {
                            Value = karat.KaratId,
                            Text = karat.Name
                        };
                        model.Add(vm);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("KaratService.ListKaratByMetalIdAsync - Exception:{@Ex}", args: new object[] { ex });
            }

            return model;
        }
    }
}
