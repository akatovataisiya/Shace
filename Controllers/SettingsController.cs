using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shace.Logic.Accounts;
using Shace.Models;
using Shace.Logic.Settings;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Diagnostics;

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
            string? photo = null;
            var image = settModel.image;
            var accInDB = _accManager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accInDB;
            await Task.Delay(0);
            if (image != null && image.Length != 0)
            {
                var supportedImageTypes = new[] { ".jpg", ".jpeg", ".png", ".tiff", ".tif", ".raw", ".dng", ".png", ".gif", ".bmp" };
                var fileExtension = Path.GetExtension(image.FileName).ToLower();
                if (supportedImageTypes.Contains(fileExtension))
                {
                    bool error = false;
                    var file = Image.FromStream(image.OpenReadStream()); //Работает только на винде
                    if (file.Width - file.Height > 10)
                    {
                        ViewBag.ErrorMsg = "• Соотношение сторон сильно отличается. Фото не квадратное";
                        error = true;
                    }
                    if (!error)
                    {
                        var newname = Convert.ToString(Guid.NewGuid());
                        var newFileName = newname + fileExtension;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Profiles/");
                        string fileNameWithPath = Path.Combine(path, newFileName);

                        // сохраняем файл в папку images/Accounts в каталоге wwwroot
                        using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            image.CopyToAsync(fileStream);
                        }
                        photo = newFileName;
                    }
                }
                else ViewBag.ErrorMsg = "• Формат файла не является изображением";

            }
            if (ModelState.IsValid)
            {
                string[] errors = new string[4];
                if (settModel.Email != null)
                {
                    if (settModel.Email != accInDB.Email)
                    {
                        if (_settManager.FindEmail(settModel.Email))
                        {
                            errors[0] = "• Почта уже занята.";
                            settModel.Email = accInDB.Email;
                        }
                    }
                }
                else
                {
                    errors[1] = "• Невозможно удалить почту.";
                    settModel.Email = accInDB.Email;
                }
                if (settModel.ShortName != null)
                {
                    if (settModel.ShortName != accInDB.ShortName)
                    {

                        if (_settManager.FindShortName(settModel.ShortName))
                        {
                            errors[2] = "• Никнейм уже занят.";
                            settModel.ShortName = accInDB.ShortName;
                        }
                    }
                }
                else
                {
                    errors[3] = "• Невозможно удалить никнейм.";
                    settModel.ShortName = accInDB.ShortName;
                }
                _settManager.ChangeAccount(settModel.Email, settModel.ShortName, settModel.Phone, settModel.Description, settModel.Location, settModel.BDay, photo, accInDB);
                if (errors[0] == null && errors[1] == null && errors[2] == null && errors[3] == null)
                    ViewBag.Complete = "✓ Настройки успешно изменены.";
                else
                    ViewBag.GlobalError = "✗ Настройки не изменены.";
                ViewBag.Errors = errors;
            }
            else
                ViewBag.GlobalError = "✗ Настройки не изменены.";
            return View("Setting");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePass(SettingsModel settModel)
        {
            await Task.Delay(0);
            var accInDB = _accManager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accInDB;
            string[] errors = new string[2];
            if (ModelState.IsValid)
            {
                if (settModel.OldPassword == accInDB.Password)
                {
                    if (settModel.OldPassword != settModel.NewPassword)
                        _settManager.ChangePassword(settModel.NewPassword, accInDB);
                    else
                        errors[0] = "• Новый пароль совпадает со старым.";
                }
                else
                    errors[1] = "• Неверно указан текущий пароль.";
                if (errors[0] == null && errors[1] == null)
                    ViewBag.Complete = "✓ Настройки успешно изменены.";
                else
                    ViewBag.GlobalError = "✗ Настройки не изменены.";
                ViewBag.ErrorsPass = errors;
            }
            else
                ViewBag.GlobalError = "✗ Настройки не изменены.";
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
            return RedirectToAction("Logout","Account");
        }

        [HttpPost]
        [Route("ChangeAcc")]
        public void ChangeAccount([FromBody] SettingsModel settModel, Account accInDB, string? photo) => _settManager.ChangeAccount(settModel.Email, settModel.ShortName, settModel.Phone, settModel.Description,  settModel.Location, settModel.BDay, photo, accInDB);

        [HttpPost]
        [Route("ChangePass")]
        public void ChangePassword([FromBody] SettingsModel settModel, Account accInDB) => _settManager.ChangePassword(settModel.NewPassword, accInDB);

        [HttpPost]
        [Route("PrTrFal")]
        public void PrivacyTrueorFalse([FromBody] SettingsModel settModel, Account accInDB) => _settManager.PrivacyTrueorFalse(settModel.Privacy, accInDB);
    }
}
