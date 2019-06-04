using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class ClientReceipt
    {
        public int ClientReceiptId { get; set; }

        public DateTime Date { get; set; }
        public string ReceiptNumber { get; set; }

        public DateTime? PaymentDate { get; set; }
        public decimal? PaymentAmount { get; set; }
        public bool? IsPaidInterestOnly { get; set; }
        public string PaymentMethod { get; set; }

        public int ClientIdentificationId { get; set; }
        public virtual ClientIdentification ClientIdentification { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public string CreatedUserId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string AuditUserId { get; set; }
        public DateTime? AuditUtc { get; set; }

        public virtual ICollection<ClientBelonging> ClientBelongings { get; set; } = new HashSet<ClientBelonging>();
    }
}
