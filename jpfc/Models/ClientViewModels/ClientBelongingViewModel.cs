using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.ClientViewModels
{
    public class ClientBelongingViewModel
    {
        [Display(Name = "Date")]
        public DateTime BelDate { get; set; }

        [Display(Name = "Action")]
        public string ClientAction { get; set; }

        [Required]
        [Display(Name = "Item")]
        public Guid MetalId { get; set; }

        [Display(Name = "Item Description")]
        public string MetalOther { get; set; }

        [Display(Name = "Purity")]
        public Guid? KaratId { get; set; }

        [Display(Name = "Purity Other")]
        public string KaratOther { get; set; }

        [Required]
        [Display(Name = "Weight in gram")]
        public decimal? Weight { get; set; }

        [Required]
        [Display(Name = "Item Price")]
        public decimal? ItemPrice { get; set; }

        [Required]
        [Display(Name = "Final Price")]
        public decimal? FinalPrice { get; set; }
    }
}
