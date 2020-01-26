using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models
{
    public class JobPost
    {
        public int JobPostId { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public Guid? JobTypeId { get; set; }
        public virtual JobType JobType { get; set; }
        public DateTime? JobStartDate { get; set; }
        public string Length { get; set; }
        public string Pay { get; set; }
        public string JobLocation { get; set; }
        public DateTime? JobCloseUtc { get; set; }
        public bool IsDraft { get; set; }
        public bool IsClosed { get; set; }

        public DateTime CreatedUtc { get; set; }
        public string CreatedUserId { get; set; }
        public string AuditUserId { get; set; }
        public DateTime? AuditUtc { get; set; }
    }
}
