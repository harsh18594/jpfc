using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.EmployeeViewModels
{
    public class CreateEmployeeViewModel
    {
        [HiddenInput]
        public string Id { get; set; }

        [Required]
        [Display(Name ="First Name")]
        [StringLength(255, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(255, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [StringLength(255, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // todo: add role
    }
}
