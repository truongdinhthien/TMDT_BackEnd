using System.ComponentModel.DataAnnotations;

namespace IdentityApi.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "You must provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber {get;set;}
        [Required]
        public string Password {get;set;}

        public bool RememberMe {get;set;}
    }
}