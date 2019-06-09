using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.ClientViewModels
{
    public class AmountSummaryViewModel
    {
        //public decimal ClientPays { get; set; }
        //public string ClientPaysStr => ClientPays.ToString("C");

        //public decimal ClientGets { get; set; }
        //public string ClientGetsStr => ClientGets.ToString("C");

        //public decimal TotalAmount => Math.Abs(ClientPays - ClientGets);
        //public string TotalAmountStr => TotalAmount.ToString("C");

        public decimal PrincipalLoanAmount { get; set; }
        public string PrincipalLoanAmountStr => PrincipalLoanAmount.ToString("C");

        public decimal InterestAmount { get; set; }
        public string InterestAmountStr => InterestAmount.ToString("C");

        public decimal PurchaseTotal { get; set; }
        public string PurchaseTotalStr => PurchaseTotal.ToString("C");

        public decimal SellTotal { get; set; }
        public string SellTotalStr => SellTotal.ToString("C");

        public decimal FinalTotal { get; set; }
        public string FinalTotalStr => Math.Abs(FinalTotal).ToString("C");

        public decimal ServiceFee { get; set; }
        public string ServiceFeeStr => ServiceFee.ToString("C");

        public decimal StorageFee { get; set; }
        public string StorageFeeStr => StorageFee.ToString("C");

        public string SummaryBlurb
        {
            get
            {
                if (FinalTotal > 0)
                {
                    return $"Client has to pay total amount of {FinalTotalStr} to JPFC";
                }
                else if (FinalTotal < 0)
                {
                    return $"JPFC has to pay total amount of {FinalTotalStr} to client";
                }
                else
                {
                    return $"Account is settled with total amount of {FinalTotalStr}";
                }
            }
        }
    }
}
