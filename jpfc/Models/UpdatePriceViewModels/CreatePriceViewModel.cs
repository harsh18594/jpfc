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
        [Display(Name = "Purity")]
        public Guid? KaratId { get; set; }

        [Required]
        [Display(Name ="Date")]
        public DateTime? Date { get; set; }

        public string DateStr => Date?.ToString("MM/dd/yyyy");

        [Required]
        [Display(Name ="Buy Price")]
        public decimal? BuyPrice { get; set; }

        [Required]
        [Display(Name = "Sell Price")]
        public decimal? SellPrice { get; set; }

        [Required]
        [Display(Name = "Loan Price")]
        public decimal? LoanPrice { get; set; }

        [Display(Name = "Loan Price Percentage")]
        public decimal? LoanPricePercent { get; set; }
    }
}
