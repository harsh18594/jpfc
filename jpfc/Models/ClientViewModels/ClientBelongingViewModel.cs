using jpfc.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.ClientViewModels
{
    public class ClientBelongingViewModel
    {
        [HiddenInput]
        public int? ClientBelongingId { get; set; }

        [HiddenInput]
        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime BelDate { get; set; }

        [Required]
        [Display(Name = " Client Action")]
        public string ClientAction { get; set; }

        [RequiredIf(Property = "MetalOther", NotHasValue = null)]
        [Display(Name = "Item")]
        public Guid? MetalId { get; set; }

        [RequiredIf(Property = "MetalId", HasValue = "other")]
        [Display(Name = "Item Description")]
        public string MetalOther { get; set; }

        [RequiredIf(Property = "MetalId", NotHasValue = "other")]
        [Display(Name = "Purity")]
        public Guid? KaratId { get; set; }

        [RequiredIf(Property = "KaratId", HasValue = "other")]
        [Display(Name = "Purity Other")]
        public string KaratOther { get; set; }

        [RequiredIf(Property = "MetalId", NotHasValue = "other")]
        [Display(Name = "Weight in gram")]
        public decimal? Weight { get; set; }

        [Required]
        [Display(Name = "Item Price")]
        public decimal? ItemPrice { get; set; }

        [Required]
        [Display(Name = "Final Price")]
        public decimal? FinalPrice { get; set; }

        public string BelDateStr => BelDate.ToString("MM-dd-yyyy");
    }
}
