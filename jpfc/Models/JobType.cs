using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class JobType
    {
        public Guid JobTypeId { get; set; }
        public string Type { get; set; }

        public virtual ICollection<JobPost> JobPosts { get; set; }
    }
}
