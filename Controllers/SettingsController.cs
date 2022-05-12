using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shace.Logic.Accounts;
using Shace.Models;
using Shace.Logic.Settings;

namespace Shace.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {

        private readonly IAccountManager _accManager;
        private readonly ISettingsManager _settManager;

        public SettingsController(IAccountManager accManager, ISettingsManager settManager)
        {
            _accManager = accManager;
            _settManager = settManager;
        }

        [HttpGet]
        public IActionResult Setting()
        {
            ViewBag.Account = _accManager.GetAccByEmail(User.Identity.Name);
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangeAcc(SettingsModel settModel)
        {
            var accInDB = _accManager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accInDB;
            await Task.Delay(0);
            if (ModelState.IsValid)
            {
                _settManager.ChangeAccount(settModel.Email, settModel.ShortName, settModel.Phone, settModel.Description, settModel.Photo, settModel.Location, settModel.BDay, accInDB);
                if (settModel.Email != null)
                {
                    if (settModel.Email != accInDB.Email)
                    {
                        var em = _settManager.FindEmail(settModel.Email);
                        if (em)
                            ModelState.AddModelError("", " Почта уже занята.");
                    }
                }
                else
                    ModelState.AddModelError("", " Невозможно удалить почту.");
                if (settModel.ShortName != null)
                {
                    if (settModel.ShortName != accInDB.ShortName)
                    {
                        var pass = _settManager.FindShortName(settModel.ShortName);
                        if (pass)
                            ModelState.AddModelError(""," Никнейм уже занят.");
                    }
                }
                else
                    ModelState.AddModelError("", " Невозможно удалить никнейм.");
            }
            return View("Setting");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePass(SettingsModel settModel)
        {
            await Task.Delay(0);
            var accInDB = _accManager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accInDB;
            if (ModelState.IsValid)
            {
                if (settModel.OldPassword == accInDB.Password)
                    if(settModel.OldPassword != settModel.NewPassword)
                        _settManager.ChangePassword(settModel.NewPassword, accInDB);
                    else
                        ModelState.AddModelError("", " Новый пароль совпадает со старым.");
                else
                    ModelState.AddModelError("", " Неверно указан текущий пароль.");
            }
            return View("Setting");
        }

        [HttpPost]
        public IActionResult PrTrFal(SettingsModel settModel)
        {
            var accInDB = _accManager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accInDB;
            _settManager.PrivacyTrueorFalse(settModel.Privacy, accInDB);
            return View("Setting");
        }

        [HttpPost]
        public IActionResult DeleteAcc()
        {
            var accInDB = _accManager.GetAccByEmail(User.Identity.Name);
            _settManager.Delete(accInDB.Id);
            return RedirectToAction("Login","Account");
        }

        [HttpPost]
        [Route("ChangeAcc")]
        public void ChangeAccount([FromBody] SettingsModel settModel, Account accInDB) => _settManager.ChangeAccount(settModel.Email, settModel.ShortName, settModel.Phone, settModel.Description, settModel.Photo, settModel.Location, settModel.BDay, accInDB);

        [HttpPost]
        [Route("ChangePass")]
        public void ChangePassword([FromBody] SettingsModel settModel, Account accInDB) => _settManager.ChangePassword(settModel.NewPassword, accInDB);

        [HttpPost]
        [Route("PrTrFal")]
        public void PrivacyTrueorFalse([FromBody] SettingsModel settModel, Account accInDB) => _settManager.PrivacyTrueorFalse(settModel.Privacy, accInDB);
    }
}
