using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class IdentificationDocument
    {
        public Guid IdentificationDocumentId { get; set; }
        public string Name { get; set; }
        public int? SortOrder { get; set; }
    }
}
