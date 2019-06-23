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
        public int ClientReceiptId { get; set; }

        [Required]
        [Display(Name = "Transaction Action")]
        public string TransactionAction { get; set; }

        [RequiredIf(Property = "MetalOther", NotHasValue = null)]
        [Display(Name = "Item")]
        public Guid? MetalId { get; set; }

        [RequiredIf(Property = "MetalId", HasValue = "other")]
        [Display(Name = "Item Other")]
        public string MetalOther { get; set; }

        [RequiredIf(Property = "MetalId", NotHasValue = "other")]
        [Display(Name = "Purity")]
        public Guid? KaratId { get; set; }

        [RequiredIf(Property = "KaratId", HasValue = "other")]
        [Display(Name = "Purity Other")]
        public string KaratOther { get; set; }

        [Display(Name = "Item Description")]
        public string ItemDescription { get; set; }

        [RequiredIf(Property = "MetalId", NotHasValue = "other")]
        [Display(Name = "Weight in gram")]
        public decimal? Weight { get; set; }

        [RequiredIf(Property = "MetalId", NotHasValue = "other")]
        [Display(Name = "Item Price")]
        public decimal? ItemPrice { get; set; }

        [Required]
        [Display(Name = "Final Price")]
        public decimal? FinalPrice { get; set; }

        [Required]
        [Display(Name = "Replacement Value")]
        public decimal? ReplacementValue { get; set; }
    }
}
