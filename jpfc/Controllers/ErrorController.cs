using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace jpfc.Controllers
{
    public class ErrorController : JpfcController
    {
        private readonly ILogger _logger;
        public ErrorController(ILogger<ErrorController> logger) 
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        public IActionResult StatusCodePage(string code = null)
        {
            _logger.LogInformation(GetLogDetails() + " - code:{@Code}", code);
            ViewData["StatusCode"] = code;
            return View();
        }
    }
}