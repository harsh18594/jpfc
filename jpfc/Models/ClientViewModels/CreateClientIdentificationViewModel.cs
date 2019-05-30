using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace jpfc.Models.ClientViewModels
{
    public class CreateClientIdentificationViewModel
    {
        [HiddenInput]
        public int ClientIdentificationId { get; set; }

        [HiddenInput]
        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Identification Document")]
        public Guid? IdentificationDocumentId { get; set; }

        [Required]
        [Display(Name = "Identification Document Number")]
        public string IdentificationDocumentNumber { get; set; }
    }
}
