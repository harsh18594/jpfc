using jpfc.Models.ClientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.ReportViewModels
{
    public class PaymentReceiptViewModel
    {
        public DateTime BillDate { get; set; }
        public string ClientName { get; set; }
        public string ClientNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }

        //public decimal BillAmount { get; set; }
        public decimal PrincipalLoanAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal StorageFee { get; set; }
        public decimal PurchaseTotal { get; set; }
        public decimal SellTotal { get; set; }
        //public decimal BrokerageFee { get; set; }
        //public decimal RetainerFee { get; set; }
        //public bool ClientPaysFinal { get; set; }
        public string PaymentMethod { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal PaymentReceived { get; set; }
        public ICollection<ClientBelongingListViewModel> Belongings { get; set; }
    }
}
