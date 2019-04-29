using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.ClientReceiptViewModels
{
    public class ClientListViewModel
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime Date { get; set; }
        public string DateStr => Date.ToString("dd/MMM/yyyy");

        public DateTime CreatedUtc { get; set; }
    }
}
