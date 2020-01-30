using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.JobPostViewModel
{
    public class JobPostDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Type { get; set; }
        public DateTime? StartDate { get; set; }
        public string Length { get; set; }
        public string Pay { get; set; }
        public string Location { get; set; }
    }
}
