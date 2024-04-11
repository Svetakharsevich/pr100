using Library.Domain.Services;
using Library.Infrastructure;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IUserService userService;
        public RegistrationController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
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
    }
}
