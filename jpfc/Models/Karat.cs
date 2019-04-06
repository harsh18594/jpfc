using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class Karat
    {
        public Guid KaratId { get; set; }
        public string Name { get; set; }
        public bool InActive { get; set; }

        public virtual ICollection<Price> Prices { get; set; }
    }
}
