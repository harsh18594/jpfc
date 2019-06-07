using jpfc.Models.ClientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.ReportViewModels
{
    public class ReceiptViewModel
    {
        public DateTime BillDate { get; set; }
        public string ClientName { get; set; }
        public string ClientNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public bool ClientPaysFinal { get; set; }
        public decimal BillAmount { get; set; }
        public ICollection<ClientBelongingListViewModel> Belongings { get; set; }
    }
}
