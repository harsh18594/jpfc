using System.ComponentModel.DataAnnotations;

namespace jpfc.Models.AccountViewModels
{
    public class AccessCodeViewModel
    {
        [Required]
        [Display(Name = "Access Code")]
        public string AccessCode { get; set; }
    }
}
