using System.ComponentModel.DataAnnotations;

namespace IdentityApiOIDC.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Fullname {get;set;}
        [Required(ErrorMessage = "You must provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber {get;set;}
        [Required]
        public string Password {get;set;}
        [Required]
        [Compare("Password",ErrorMessage = "Password and Confirmpassword do not match")]
        public string ConfirmPassword {get;set;}
        public string ReturnUrl { get; set; }
    }
}