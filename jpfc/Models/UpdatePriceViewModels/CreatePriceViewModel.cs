using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.UpdatePriceViewModels
{
    public class CreatePriceViewModel
    {
        [HiddenInput]
        public int PriceId { get; set; }

        [Required]
        [Display(Name = "Metal")]
        public Guid MetalId { get; set; }

        //[Required]
        [Display(Name = "Karat")]
        public Guid? KaratId { get; set; }

        [Required]
        [Display(Name ="Date")]
        public DateTime? Date { get; set; }

        public string DateStr => Date?.ToString("MM/dd/yyyy");

        [Required]
        public decimal? Amount { get; set; }
    }
}
