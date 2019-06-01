using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.ClientViewModels
{
    public class ClientReceiptViewModel
    {
        public int ClientId { get; set; }
        public int ClientReceiptId { get; set; }

        public string ReceiptNumber { get; set; }

        public DateTime Date { get; set; }
        public string DateStr => Date.ToString("MM/dd/yyyy");

        public decimal? Amount { get; set; }
        public string AmountStr => Amount?.ToString("C");
    }
}
