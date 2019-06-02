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
        
        public DateTime? PaymentDate { get; set; }
        public string PaymentDateStr => PaymentDate?.ToString("MM/dd/yyyy");

        public decimal? PaymentAmount { get; set; }
        public string PaymentAmountStr => PaymentAmount?.ToString("C");

        public DateTime Date { get; set; }
        public string DateStr => Date.ToString("MM/dd/yyyy");

        public decimal BusinessGetsAmount { get; set; }
        public decimal BusinessPaysAmount { get; set; }

        public string FinalAmountStr
        {
            get
            {
                var retVal = "";
                var diff = Math.Abs(BusinessGetsAmount - BusinessPaysAmount);
                if (BusinessGetsAmount > BusinessPaysAmount)
                {
                    retVal = $"{diff.ToString("C")} [CR]";
                }
                else
                {
                    retVal = $"{diff.ToString("C")} [DR]";
                }
                return retVal;
            }
        }
    }
}
