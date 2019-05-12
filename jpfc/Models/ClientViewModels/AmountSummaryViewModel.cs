using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.ClientViewModels
{
    public class AmountSummaryViewModel
    {
        public decimal ClientPays { get; set; }
        public string ClientPaysStr => ClientPays.ToString("C");

        public decimal ClientGets { get; set; }
        public string ClientGetsStr => ClientGets.ToString("C");

        public decimal TotalAmount => Math.Abs(ClientPays - ClientGets);
        public string TotalAmountStr => TotalAmount.ToString("C");

        public string SummaryBlurb
        {
            get
            {
                if (ClientPays > ClientGets)
                {
                    return $"Client has to pay total amount of {TotalAmountStr} to JPFC";
                }
                else if (ClientPays < ClientGets)
                {
                    return $"JPFC has to pay total amount of {TotalAmountStr} to client";
                }
                else
                {
                    return $"Account is settled with total amount of {TotalAmountStr}";
                }
            }
        }
    }
}
