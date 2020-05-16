using System.ComponentModel.DataAnnotations;

namespace IdentityApiOIDC.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "You must provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber {get;set;}
        [Required]
        [DataType(DataType.Password)]
        public string Password {get;set;}
        [Display(Name = "Remember Me")]
        public bool RememberMe {get;set;}

        public string ReturnUrl { get; set; }
    }
}