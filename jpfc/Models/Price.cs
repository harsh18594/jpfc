using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class Price
    {
        public int PriceId { get; set; }

        public DateTime Date { get; set; }

        public Guid MetalId { get; set; }
        public virtual Metal Metal { get; set; }

        public Guid? KaratId { get; set; }
        public virtual Karat Karat { get; set; }

        public decimal? BuyPrice { get; set; }
        public decimal? SellPrice { get; set; }
        public decimal? LoanPricePercent { get; set; }
        public decimal? LoanPrice { get; set; }

        public DateTime CreatedUtc { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime? AuditUtc { get; set; }
        public string AuditUserId { get; set; }
    }
}
