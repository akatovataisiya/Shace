using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shace.Models;
using Microsoft.AspNetCore.Authorization;
using Shace.Logic.Accounts;
using Shace.Logic.Posts;

namespace Shace.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AccountContext _context;
        private readonly IAccountManager _accmanager;
        private readonly ILogger<HomeController> _logger;
        private readonly IPostManager _postmanager;
        private readonly IWebHostEnvironment _appEnvironment;

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
            if (image != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                string fileNameWithPath = Path.Combine(path, image.FileName);
                // сохраняем файл в папку images в каталоге wwwroot
                using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    image.CopyToAsync(fileStream);
                }
                var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
                // установка массива байтов
                _post.Photo = image.FileName;
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
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}