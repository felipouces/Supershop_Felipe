using System.ComponentModel.DataAnnotations;

namespace Supershop.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required] // Password is required
        [MinLength(6)] // Minimum length requirement for password
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
