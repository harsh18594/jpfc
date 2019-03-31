using System.ComponentModel.DataAnnotations;

namespace jpfc.Models.ManageViewModels
{
    public class AccessCodeViewModel
    {
        [Required]
        [Display(Name = "Current Access Code")]
        public string CurrentAccessCode { get; set; }

        [Required]
        [Display(Name = "New Access Code")]
        public string NewAccessCode { get; set; }

        public string StatusMessage { get; set; }
    }
}
