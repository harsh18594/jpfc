using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace jpfc.Controllers
{
    public class ServicesController : JpfcController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BuySellGold()
        {
            return View();
        }

        public IActionResult CreditCards()
        {
            return View();
        }

        public IActionResult Insurance()
        {
            return View();
        }

        public IActionResult Loan()
        {
            return View();
        }

        public IActionResult Mortgage()
        {
            return View();
        }

        public IActionResult FinancePlanning()
        {
            return View();
        }
    }
}