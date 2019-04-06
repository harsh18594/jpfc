using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.UpdatePriceViewModels
{
    public class PriceListViewModel
    {
        public int PriceId { get; set; }
        public DateTime Date { get; set; }
        public string DateStr => Date.ToString("dd/MMM/yyyy");

        public decimal Amount { get; set; }
        public string Metal { get; set; }
        public string Karat { get; set; }

        public DateTime CreatedUtc { get; set; }
    }
}
