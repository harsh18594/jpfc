using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.MortgageViewModels
{
    public class CreateMortgageRateViewModel
    {
        [Display(Name = "Rate")]
        public decimal? Rate { get; set; }
    }
}
