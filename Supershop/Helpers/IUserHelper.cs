using Microsoft.AspNetCore.Identity;
using Supershop.Data.Entities;
using System.Threading.Tasks;

namespace Supershop.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);
    }
}
