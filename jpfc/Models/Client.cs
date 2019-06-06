using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public string AddressEncrypted { get; set; }
        public string AddressUniqueKey { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumberEncrypted { get; set; }
        public string ContactNumberUniqueKey { get; set; }

        public string CreatedUserId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string AuditUserId { get; set; }
        public DateTime? AuditUtc { get; set; }

        public virtual ICollection<ClientIdentification> Identifications { get; set; } = new HashSet<ClientIdentification>();
        public virtual ICollection<ClientReceipt> Receipts { get; set; } = new HashSet<ClientReceipt>();
    }
}
