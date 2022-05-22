using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shace.Logic.Accounts;
using Shace.Logic.Posts;
using Shace.Logic.ProfilePage;
using System.Drawing;

namespace Shace.Controllers
{
    [Authorize]
    public class ProfilePageController : Controller
    {
        private readonly IAccountManager _accmanager;
        private readonly IPostManager _postmanager;
        private readonly IProfilePageManager _ppmanager;
        public ProfilePageController(IAccountManager accmanager, IPostManager postmanager, IProfilePageManager ppmanager)
        {
            _accmanager = accmanager;
            _postmanager = postmanager;
            _ppmanager = ppmanager;
        }

        [HttpGet]
        public IActionResult Profile(string id)
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accountInDb;
            if (id != accountInDb.ShortName)
            {
                var pageowner = _ppmanager.GetAcc(id);
                ViewBag.Profile = pageowner;
                ViewBag.Subscription = _ppmanager.IfSub(accountInDb, pageowner);
                ViewBag.Posts = _ppmanager.GetPosts(pageowner);
            }
            else
            {
                ViewBag.Profile = accountInDb;
                ViewBag.Posts = _ppmanager.GetPosts(accountInDb);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Subscriptions(string snProf)
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            var pageowner = _ppmanager.GetAcc(snProf);
            _ppmanager.Subscribe(accountInDb, pageowner);
            return RedirectToAction("Profile", "ProfilePage", new { id = pageowner.ShortName });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accountInDb;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Advertisment _ad, IFormFile image)
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accountInDb;

            if (image != null && image.Length != 0)
            {
                var supportedImageTypes = new[] { ".jpg", ".jpeg", ".png", ".tiff", ".tif", ".raw", ".dng", ".png", ".gif", ".bmp" };
                var supportedVideoTypes = new[] { ".avi", ".mp4", ".wmv", ".m4v", ".mov", ".mpeg", ".mpg" };
                var fileExtension = Path.GetExtension(image.FileName).ToLower();
                if (supportedImageTypes.Contains(fileExtension) || supportedVideoTypes.Contains(fileExtension))
                {
                    if (supportedImageTypes.Contains(fileExtension))
                    {
                        var file = Image.FromStream(image.OpenReadStream()); //Работает только на винде
                        if (file.Width < 450)
                        {
                            ViewBag.ErrorMsg = "• Размер фото меньше 450x450";
                            return View();
                        }
                        else if (file.Height < 450)
                        {
                            ViewBag.ErrorMsg = "• Размер фото меньше 450x450";
                            return View();
                        }
                        else if (file.Width - file.Height > 10)
                        {
                            ViewBag.ErrorMsg = "• Соотношение сторон сильно отличается. Фото не квадратное";
                            return View();
                        }
                    }

                    var newname = Convert.ToString(Guid.NewGuid());
                    var newFileName = newname + fileExtension;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/UAds/");
                    string fileNameWithPath = Path.Combine(path, newFileName);

                    // сохраняем файл в папку images/Posts в каталоге wwwroot
                    using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        image.CopyToAsync(fileStream);
                    }

                    _ad.Url = newFileName;
                    _ad.AccountId = accountInDb.Id;
                    _ppmanager.CreateAd(_ad);
                    return RedirectToAction("Profile", "ProfilePage", new { id = accountInDb.ShortName });
                }
                else ViewBag.ErrorMsg = "• Формат файла не является изображением/видео";
                return View();

            }
            ViewBag.ErrorMsg = "• Вы не добавили файл";
            return View();
        }

        [HttpGet]
        public IActionResult Post(int id)
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accountInDb;
            ViewBag.Post = _postmanager.GetPost(id);
            ViewBag.PostAcc = _postmanager.GetAcc(ViewBag.Post.AccountId);
            ViewBag.Comments = _postmanager.GetComments(id);
            ViewBag.AllAccs = _postmanager.GetAccs();
            ViewBag.Likes = _postmanager.GetLikes(id);
            return View();
        }

        [HttpGet]
        public IActionResult UpdatePost(int postid)
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accountInDb;
            ViewBag.Post = _postmanager.GetPost(postid);
            return View();
        }

        [HttpPost]
        public IActionResult UpdatePost(string? description, string? location, int postid)
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            _postmanager.UpdatePost(description, location, postid);
            return RedirectToAction("Post", "ProfilePage", new { id = postid});
        }

        [HttpPost]
        public IActionResult DeletePost(int postid)
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            _postmanager.DeletePost(postid);
            return RedirectToAction("Profile", "ProfilePage", new { id = accountInDb.ShortName });
        }
    }
}
