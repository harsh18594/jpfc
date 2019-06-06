using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class ClientIdentification
    {
        public int ClientIdentificationId { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public Guid? IdentificationDocumentId { get; set; }
        public virtual IdentificationDocument IdentificationDocument { get; set; }
        public string IdentificationDocumentNumberEncrypted { get; set; }
        public string IdentificaitonDocumentNumberUniqueKey { get; set; }

        public string CreatedUserId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string AuditUserId { get; set; }
        public DateTime? AuditUtc { get; set; }

        public virtual ICollection<ClientReceipt> ClientReceipts { get; set; } = new HashSet<ClientReceipt>();
    }
}
