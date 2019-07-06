using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class ClientBelonging
    {
        public int ClientBelongingId { get; set; }
        
        public string TransactionAction { get; set; }

        public int ClientReceiptId { get; set; }
        public virtual ClientReceipt ClientReceipt { get; set; }

        public Guid? MetalId { get; set; }
        public virtual Metal Metal { get; set; }
        public string MetalOther { get; set; }

        public string ItemDescription { get; set; }

        public Guid? KaratId { get; set; }
        public virtual Karat Karat { get; set; }
        public string KaratOther { get; set; }

        public decimal? ItemWeight { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? FinalPrice { get; set; }
        public decimal? ReplacementValue { get; set; }
        public decimal? HstAmount { get; set; }

        public string CreatedUserId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string AuditUserId { get; set; }
        public DateTime? AuditUtc { get; set; }
    }
}
