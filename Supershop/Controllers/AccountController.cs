using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Supershop.Helpers;
using Supershop.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Supershop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        public AccountController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) // POST: /Account/Login
        {
            if (ModelState.IsValid) // Verifica se os dados do modelo são válidos
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded) // Verifica se o login foi bem-sucedido
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl")) // Logou atraves de uma URL de retorno
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login!");
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
