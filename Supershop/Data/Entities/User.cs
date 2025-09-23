using Microsoft.AspNetCore.Identity;

namespace Supershop.Data.Entities
{
    public class User : IdentityUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        // Since I'll have to use authentication, I can't use my simple DataContext.
        // I have authentication, and it will be different.

        public string FullName => $"{FirstName} {LastName}";
    }
}
