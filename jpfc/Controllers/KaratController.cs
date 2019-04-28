using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Controllers
{
    public class KaratController : JpfcController
    {
        private readonly ILogger _logger;
        private readonly IKaratService _karatService;

        public KaratController(ILogger<KaratController> logger,
            IKaratService karatService)
        {
            _logger = logger;
            _karatService = karatService;
        }

        public async Task<IActionResult> FetchKarats(Guid? metalId)
        {
            _logger.LogInformation(GetLogDetails() + " - metalId:{@MetalId}", args: new object[] { metalId });

            var result = await _karatService.ListKaratByMetalIdAsync(metalId);
            return Json(new
            {
                model = result
            });
        }
    }
}
