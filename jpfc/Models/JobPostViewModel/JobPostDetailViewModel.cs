using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.JobPostViewModel
{
    public class JobPostDetailViewModel
    {
        public int JobPostId { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string JobType { get; set; }
        public DateTime? JobStartDate { get; set; }
        public string Length { get; set; }
        public string Pay { get; set; }
        public string JobLocation { get; set; }
        public DateTime? JobCloseUtc { get; set; }
        public bool IsDraft { get; set; }
        public bool IsClosed { get; set; }
    }
}
