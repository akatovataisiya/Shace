using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shace.Logic.Accounts;
using Shace.Logic.Posts;
using Shace.Logic.ProfilePage;

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
                var accountInDbUser = _ppmanager.GetAcc(id);
                ViewBag.Profile = accountInDbUser;
                ViewBag.Subscription = _ppmanager.IfSub(accountInDb, accountInDbUser);
                ViewBag.Posts = _ppmanager.GetPosts(accountInDbUser);
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
            var accountInDb=_ppmanager.GetAcc(User.Identity.Name);
            var accountInDbUser=_ppmanager.GetAcc(snProf);
            _ppmanager.Subscribe(accountInDb, accountInDbUser);
            return RedirectToAction("Profile", "ProfilePage", new { id = accountInDbUser.ShortName }) ;
        }
    }
}
