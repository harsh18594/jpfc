using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class MortgageRate
    {
        public int MortgageRateId { get; set; }
        public decimal? Rate { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime? AuditUtc { get; set; }
        public string AuditUserId { get; set; }
    }
}
