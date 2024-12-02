﻿using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.ViewModel
{
    public class Loginviewmodel
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage ="Email Address is Required")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
