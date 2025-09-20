using Microsoft.AspNetCore.Identity;
using Supershop.Data.Entities;
using Supershop.Models;
using System.Threading.Tasks;

namespace Supershop.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        Task CheckRoleAsync(string roleName); // Ensure the role exists


        Task AddUserToRoleAsync(User user, string roleName); // Assign a role to a user


        Task<bool> IsUserInRoleAsync(User user, string roleName); // Check if a user is in a specific role
    }
}
