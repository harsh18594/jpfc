using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.JobPostViewModel
{
    public class CreateJobPostViewModel
    {
        [HiddenInput]
        public int? JobPostId { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(255, ErrorMessage = "{0} cannot exceed {1} characters.")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Requirements")]
        public string Requirements { get; set; }

        [Required]
        [Display(Name = "Job Type")]
        public Guid? JobTypeId { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? JobStartDate { get; set; }

        [Display(Name = "Length")]
        [StringLength(500, ErrorMessage = "{0} cannot exceed {1} characters.")]
        public string Length { get; set; }

        [Display(Name = "Pay")]
        [StringLength(500, ErrorMessage = "{0} cannot exceed {1} characters.")]
        public string Pay { get; set; }

        [Required]
        [Display(Name = "Location")]
        [StringLength(500, ErrorMessage = "{0} cannot exceed {1} characters.")]
        public string JobLocation { get; set; }

        [Display(Name = "Post Closing Date")]
        public DateTime? JobCloseDate { get; set; }

        [Required]
        [Display(Name = "Is Draft?")]
        public bool IsDraft { get; set; }

        [Required]
        [Display(Name = "Is Closed?")]
        public bool IsClosed { get; set; }
    }
}
