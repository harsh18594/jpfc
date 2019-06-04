using jpfc.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.ClientViewModels
{
    public class CreateClientReceiptViewModel
    {
        [HiddenInput]
        public int? ClientReceiptId { get; set; }

        [HiddenInput]
        public int ClientId { get; set; }

        [Display(Name = "Receipt Number")]
        public string ReceiptNumber { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public string CreatedDateTimeStr => CreatedDateTime.ToString("MM/dd/yyyy HH:mm:ss");

        [Display(Name = "Payment Date")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Payment Amount")]
        public decimal? PaymentAmount { get; set; }

        [RequiredIf(Property = "IdentificationDocumentId", HasValue = "")]
        [Display(Name = "Saved Identification")]
        public int? ClientIdentificationId { get; set; }

        [RequiredIf(Property = "ClientIdentificationId", HasValue = "")]
        [Display(Name = "Identification Document")]
        public Guid? IdentificationDocumentId { get; set; }

        [RequiredIf(Property = "ClientIdentificationId", HasValue = "")]
        [Display(Name = "Identification Document Number")]
        public string IdentificationDocumentNumber { get; set; }

        public ClientBelongingViewModel ClientBelongingViewModel { get; set; }


        // readonly information
        public string ClientName { get; set; }
        public string ClientNumber { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
    }
}
