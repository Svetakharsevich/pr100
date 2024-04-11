using Library.Domain.Entities;
using Library.Domain.Services;
using Library.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace Library.Controllers
{
    public class UserController : Controller
    {
        private const int adminRoleId = 2;
        private const int clientRoleId = 1;
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        private async Task SignIn(User user)
        {
            string role = user.RoleId switch
            {
                adminRoleId => "admin",
                clientRoleId => "client",
                _ => throw new ApplicationException("invalid user role")
            };
            List<Claim> claims = new List<Claim>
            {   
                 new Claim("fullname", user.Fullname),
                 new Claim("id", user.Id.ToString()),
                 new Claim("role", role),
                 new Claim("username", user.Login)
            };
            string authType = CookieAuthenticationDefaults.AuthenticationScheme;
            IIdentity identity = new ClaimsIdentity(claims, authType, "username", "role");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> LoginAsync(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = await userService.GetUserAsync(loginViewModel.Username, loginViewModel.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль");
                return View(loginViewModel);
            }

            await SignIn(user);

            return RedirectToAction("Index", "Books");
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel registration)
        {
            if (!ModelState.IsValid)
            {
                return View(registration);
            }

            if (await userService.IsUserExistsAsync(registration.Username))
            {
                ModelState.AddModelError("user_exists", $"Имя пользователя {registration.Username} уже существует!");
                return View(registration);
            }


            try
            {
                await userService.RegistrationAsync(registration.Fullname, registration.Username, registration.Password);
                return RedirectToAction("RegistrationSuccess", "User");
            }
            catch
            {
                ModelState.AddModelError("reg_error", $"Не удалось зарегистрироваться, попробуйте попытку регистрации позже");
                return View(registration);
            }
        }
        public IActionResult RegistrationSuccess()
        {
            return View();
        }
    }
}
