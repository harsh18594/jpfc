using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class AccessCode
    {
        public Guid AccessCodeId { get; set; }
        
        public string EncryptedValue { get; set; }
        public string UniqueKey { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? AuditUtc { get; set; }
    }
}
