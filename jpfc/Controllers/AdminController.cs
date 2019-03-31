using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Controllers
{
    public class AdminController : JpfcController
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
