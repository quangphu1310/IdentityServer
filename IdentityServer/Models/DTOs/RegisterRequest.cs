using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Username must be a valid email address.")]
        public string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword do not match.")]
        public string ConfirmPassword { get; set; }
    }

}
