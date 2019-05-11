using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.ClientViewModels
{
    public class ClientBelongingListViewModel
    {
        public int ClientBelongingId { get; set; }
        public string Metal { get; set; }
        public string Karat { get; set; }

        public decimal? Weight { get; set; }
        public string WeightStr => Weight?.ToString("#.00");

        public decimal? ItemPrice { get; set; }
        public string ItemPriceStr => ItemPrice?.ToString("C");

        public decimal? FinalPrice { get; set; }
        public string FinalPriceStr => FinalPrice?.ToString("C");

        public DateTime CreatedUtc { get; set; }
    }
}
