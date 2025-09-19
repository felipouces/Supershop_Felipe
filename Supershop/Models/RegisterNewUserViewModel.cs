using System.ComponentModel.DataAnnotations;

namespace Supershop.Models
{
    public class RegisterNewUserViewModel
    {
        [Required] // Ensures the field is not left empty
        [Display(Name = "First Name")]  
        public string FirstName { get; set; }

        [Required] // Ensures the field is not left empty
        [Display(Name = "Last Name")] 
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)] // Ensures the input is a valid email format
        public string Username { get; set; }

        [Required]
        [MinLength(6)] // Ensures the password is at least 6 characters long
        public string Password { get; set; }

        [Required]
        [Compare("Password")] // Ensures this field matches the Password field
        public string Confirm { get; set; }
    }
}
