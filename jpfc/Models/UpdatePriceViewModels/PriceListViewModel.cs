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

        public decimal? BuyPrice { get; set; }
        public string BuyPriceStr => BuyPrice?.ToString("C");

        public decimal? SellPrice { get; set; }
        public string SellPriceStr => SellPrice?.ToString("C");

        public decimal? LoanPrice { get; set; }
        public string LoanPriceStr => LoanPrice?.ToString("C");

        public string Metal { get; set; }
        public string Karat { get; set; }
        public bool PerOunce { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
