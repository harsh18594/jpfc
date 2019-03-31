using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class AccessCode
    {
        public Guid AccessCodeId { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public string PlainTextCode { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
