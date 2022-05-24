namespace Shace.Logic.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private readonly AccountContext _context;

        public SettingsManager(AccountContext context)
        {
            _context = context;
        }
        public void ChangeAccount(string email, string shortName, long? phone, string? description, string? location, DateTime? bDay, string? photo, Account account)
        {
            account.BDay = bDay;
            if (photo != null)
                account.Photo = photo;
            account.Location = location;
            account.Description = description;
            account.Phone = phone;
            account.Email = email;
            account.ShortName = shortName;
            account.Url = $"https://shace.com/{account.ShortName}";
            _context.SaveChanges();
        }

        public bool FindEmail(string email)
        {
            var accountInDb = _context.Accounts.FirstOrDefault(acc => (acc.Email == email));
            if (accountInDb == null)
                return false;
            return true;
        }

        public bool FindShortName(string shortName)
        {
            var accountInDb = _context.Accounts.FirstOrDefault(acc => (acc.ShortName == shortName));
            if (accountInDb == null)
                return false;
            return true;
        }

        public void ChangePassword(string? password, Account account)
        {
            if(password!=null)
                account.Password = password;
            _context.SaveChanges();
        }

        public void PrivacyTrueorFalse(bool privacy, Account account)
        {
            account.Privacy = privacy;
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account != null)
            {
                var liker = _context.Likes.Where(l => l.AccountId == account.Id);
                foreach (var deleter in liker)
                    _context.Likes.Remove(deleter);
                var commenter = _context.Comments.Where(comment => comment.AccountId == account.Id);
                foreach (var deleter in commenter)
                    _context.Comments.Remove(deleter);
                var poster = _context.Posts.Where(post => post.AccountId == account.Id);
                foreach (var deleter in poster)
                    _context.Posts.Remove(deleter);
                var dialoger = _context.Dialogs.Where(dialog => dialog.AccountId == account.Id || dialog.Account2Id == account.Id);
                foreach (var deleter in dialoger)
                {
                    var messager = _context.Messages.Where(message => message.DialogId == deleter.Id);
                    foreach (var message_deleter in messager)
                        _context.Messages.Remove(message_deleter);
                    _context.Dialogs.Remove(deleter);
                }
                var suber = _context.Subscribtions.Where(sub => sub.AccountId == account.Id || sub.AccountId == account.Id);
                foreach (var deleter in suber)
                    _context.Subscribtions.Remove(deleter);
                var adver = _context.Advertisments.Where(advert => advert.AccountId == account.Id);
                foreach (var deleter in adver)
                    _context.Advertisments.Remove(deleter);
                _context.Accounts.Remove(account);
                foreach (var acc in _context.Accounts)
                { 
                    var subers = _context.Subscribtions.Where(sub=> sub.Account2Id == acc.Id).ToList();
                    acc.SubscibersCounter = subers.Count;
                    subers = _context.Subscribtions.Where(sub => sub.AccountId == acc.Id).ToList();
                    acc.SubscriptionsCounter = subers.Count;
                }
                foreach (var post in _context.Posts)
                {
                    var likes = _context.Likes.Where(l => l.PostId == post.Id).ToList();
                    post.LikeCounter = likes.Count;
                    var comments = _context.Comments.Where(c => c.PostId == post.Id).ToList();
                    post.CommentCounter = comments.Count;
                }
                _context.SaveChanges();
            }

        }

    }
}
