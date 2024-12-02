using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.ViewModel
{
    public class RegisterViewModel
    {
        [Display(Name ="Email Address")]
        [Required(ErrorMessage ="Email Address is Required")]
        public string EmailAddress { get; set; }
        
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [Required(ErrorMessage ="Confirm Password is Required")]

        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password",ErrorMessage ="Password do Not match")]
        public string ConfirmPassword { get; set; }


    }
}
