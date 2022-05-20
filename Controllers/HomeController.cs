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
            ViewBag.Account = accountInDb;
            ViewBag.Likes = _context.Likes;
            ViewBag.Adds = _context.Advertisments;
            ViewBag.AddsCount = _context.Advertisments.Count();
            ViewBag.Subs = _context.Subscribtions.Where(sub => sub.AccountId == accountInDb.Id);
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

        [HttpPost]
        public IActionResult Liked(int postid, int accountid)
        {
            Like _like = new Like();
            var postInDb = _context.Posts.Where(post => post.Id == postid).FirstOrDefault();
            var alreadyLiked = _context.Likes.FirstOrDefault(liker => liker.PostId == postid && liker.AccountId == accountid);
            if (alreadyLiked == null)
            {
                _like.PostId = postid;
                postInDb.LikeCounter += 1;
                _like.AccountId = accountid;
                _context.Likes.Add(_like);
                _context.SaveChanges();
            }
            else
            {
                postInDb.LikeCounter -= 1;
                _context.Likes.Remove(alreadyLiked);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Commented(int postid, int accountid, string text)
        {
            Comment _comm = new Comment();
            var postInDb = _context.Posts.Where(post => post.Id == postid).FirstOrDefault();
            postInDb.CommentCounter += 1;
            _comm.CommentDate = DateTime.Now;
            _comm.PostId = postid;
            _comm.Text = text;
            _comm.AccountId = accountid;
            _context.Comments.Add(_comm);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search()
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accountInDb;
            return View();
        }

        [HttpPost]
        public IActionResult Search(string search)
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            ViewBag.Account = accountInDb;
            var accInDBSearch = _context.Accounts.Where(s => s.ShortName.Contains(search));
            var searchAcc = accInDBSearch.OrderByDescending(s => s.ShortName.StartsWith(search)).ToList();
            var searchUserAcc = searchAcc.FirstOrDefault(s => s.Email == User.Identity.Name);
            if (searchUserAcc != null)
                searchAcc.Remove(searchUserAcc);
            if (searchAcc.Count > 20)
                searchAcc.RemoveRange(20, searchAcc.Count - 20);
            ViewBag.AllAccs = searchAcc;
            ViewBag.Search = search;
            return View();
        }

        [HttpGet]
        public IActionResult Comments(int postid)
        {
            var accountInDb = _accmanager.GetAccByEmail(User.Identity.Name);
            var post = _context.Posts.FirstOrDefault(p => p.Id == postid);
            var postowner = _context.Accounts.FirstOrDefault(a => a.Id == post.AccountId);
            ViewBag.Account = accountInDb;
            ViewBag.AllAccs = _context.Accounts;
            ViewBag.PostOwner = postowner;
            ViewBag.PostDescr = post.Description;
            var comments = _context.Comments.Where(c => c.PostId == postid).ToList();
            comments.Reverse();
            return View(comments);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
