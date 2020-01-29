using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.JobPostViewModel
{
    public class JobPostListViewModel
    {
        public int JobPostId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public bool IsDraft { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateStr => StartDate?.ToString("MMM dd, yyyy");
    }
}
