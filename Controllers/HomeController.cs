using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shace.Models;
using Microsoft.AspNetCore.Authorization;
using Shace.Logic.Accounts;
using Shace.Logic.Posts;
using System.Drawing; //Вытягивать размеры сторон изображения

namespace Shace.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AccountContext _context;
        private readonly IAccountManager _accmanager;
        private readonly ILogger<HomeController> _logger;
        private readonly IPostManager _postmanager;

        public HomeController(ILogger<HomeController> logger, IAccountManager accmanager, AccountContext context)
        {
            _logger = logger;
            _accmanager = accmanager;
            _context = context;
        }

        public IActionResult Index()
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            ViewBag.AllAccs = _context.Accounts;
            ViewBag.Account=accountInDb;
            return View(_context.Posts);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create()
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            ViewBag.AllAccs = _context.Accounts;
            ViewBag.Account = accountInDb;
            return View(_context.Posts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Post _post, IFormFile image)
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accountInDb;

            if (image != null && image.Length != 0)
            {
                var supportedImageTypes = new[] { ".jpg", ".jpeg", ".png", ".tiff", ".tif", ".raw", ".dng", ".png", ".gif", ".bmp" };
                var supportedVideoTypes = new[] { ".avi", ".mp4", ".wmv", ".m4v", ".mov", ".mpeg", ".mpg" };
                var fileExtension = Path.GetExtension(image.FileName);
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
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Posts/");
                    string fileNameWithPath = Path.Combine(path, newFileName);

                    // сохраняем файл в папку images/Posts в каталоге wwwroot
                    using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        image.CopyToAsync(fileStream);
                    }

                    _post.Photo = newFileName;
                    _post.AccountId = accountInDb.Id;
                    _post.PostDate = DateTime.Now;
                    _post.LikeCounter = 0;
                    _post.CommentCounter = 0;
                    _post.MarkCounter = 0;
                    _post.PostType = 0;
                    _context.Posts.Add(_post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else ViewBag.ErrorMsg = "• Формат файла не является изображением/видео";
                return View();

            }
            ViewBag.ErrorMsg = "• Вы не добавили файл";
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
