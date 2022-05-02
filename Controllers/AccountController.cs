using Microsoft.AspNetCore.Mvc;
using Shace.Logic.Accounts;
using Shace.Models;

namespace Shace.Controllers
{
    public class AccountController : Controller
    {
        private IAccountManager _manager;

        public AccountController(IAccountManager manager) => _manager = manager;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var loginAccount = await _manager.SignIn(loginModel.Email, loginModel.Password);
                if (loginAccount)
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                var accountInDb = await _manager.Find(loginModel.Email);
                if (accountInDb)
                    ModelState.AddModelError("", "Неверно указан пароль");
                else
                    ModelState.AddModelError("", "Аккаунта не существует");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var createAccount = await _manager.Create(registerModel.Email, registerModel.ShortName, registerModel.Password);
                if (createAccount)
                    return Redirect("Login");
                var accountInDb = await _manager.Find(registerModel.Email);
                if (accountInDb)
                    ModelState.AddModelError("", "Почта уже используется");
                else
                    ModelState.AddModelError("", "Никнейм уже используется");
            }
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public Task<bool> SignIn([FromBody] LoginModel loginModel) =>  _manager.SignIn(loginModel.Email, loginModel.Password);


        [HttpPost]
        [Route("Register")]
        public Task<bool> Create([FromBody] RegisterModel registerModel) => _manager.Create(registerModel.Email, registerModel.ShortName, registerModel.Password);
    }
}
