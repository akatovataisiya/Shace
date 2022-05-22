namespace Shace.Logic.ProfilePage
{
    public class ProfilePageManager : IProfilePageManager
    {
        private readonly AccountContext _context;

        public ProfilePageManager(AccountContext context)
        {
            _context = context;
        }

        public Account GetAcc(string shortName)
        {
            var accountInDb = _context.Accounts.FirstOrDefault(acc => acc.ShortName == shortName);
            if (accountInDb != null)
                return accountInDb;
            else return null;
        }

        public List<Post> GetPosts(Account account)
        {
            var posts = _context.Posts.Where(p => p.AccountId == account.Id).ToList();
            posts.Reverse();
            return posts;
        }

        public Subscribtion IfSub(Account accountInDb, Account accountInDbUser)
        {
            var oneAccInDbSub= _context.Subscribtions.FirstOrDefault(s=>(s.AccountId==accountInDb.Id && s.Account2Id==accountInDbUser.Id));
            if (oneAccInDbSub != null)
                return oneAccInDbSub;
            else return null;
        }

        public void Subscribe(Account accountInDb, Account accountInDbUser)
        {
            var sub = IfSub(accountInDb, accountInDbUser);
            if (sub == null)
            {
                Subscribtion subscribtion = new Subscribtion();
                subscribtion.AccountId = accountInDb.Id;
                subscribtion.Account2Id = accountInDbUser.Id;
                accountInDb.SubscriptionsCounter += 1;
                accountInDbUser.SubscibersCounter += 1;
                _context.Subscribtions.Add(subscribtion);
            }
            else
            {
                accountInDb.SubscriptionsCounter -= 1;
                accountInDbUser.SubscibersCounter -= 1;
                _context.Subscribtions.Remove(sub);
            }
            _context.SaveChanges();
        }

        public void CreateAd(Advertisment _ad)
        {
            _context.Advertisments.Add(_ad);
            _context.SaveChanges();
        }

        public Dialog IfDial(Account accountInDb, Account accountInDbUser)
        {
            var oneAccInDbDial = _context.Dialogs.FirstOrDefault(s => (s.AccountId == accountInDb.Id && s.Account2Id == accountInDbUser.Id || s.Account2Id == accountInDb.Id && s.AccountId == accountInDbUser.Id));
            if (oneAccInDbDial != null)
                return oneAccInDbDial;
            else return null;
        }

        public int Dialog(Account accountInDb, Account accountInDbUser)
        {
            var dial = IfDial(accountInDb, accountInDbUser);
            if (dial == null)
            {
                var dialog = new Dialog();
                dialog.AccountId = accountInDb.Id;
                dialog.Account2Id = accountInDbUser.Id;
                _context.Dialogs.Add(dialog);
                _context.SaveChanges();
                return dialog.Id;
            }
            return dial.Id;
        }

        public List<Account> GetDial(Account accountInDb)
        {
            var dialogs = _context.Dialogs.Where(d => (d.AccountId == accountInDb.Id || d.Account2Id == accountInDb.Id)).ToList();
            var users = new List<Account>();
            foreach (var dial in dialogs)
            {
                users.Add(_context.Accounts.FirstOrDefault(a => (a.Id != accountInDb.Id && (a.Id == dial.AccountId || a.Id == dial.Account2Id))));
            }
            return users;
        }

        public List<Message> GetMes(Account accountInDb, Account accountInDbUser, int dialId)
        {
            var mesacc1 = _context.Messages.Where(m => (m.DialogId == dialId && m.AccountId == accountInDb.Id));
            var mesacc2 = _context.Messages.Where(m => (m.DialogId == dialId && m.AccountId == accountInDbUser.Id));
            mesacc1.ToList();
            mesacc2.ToList();
            var messages = mesacc1.Concat(mesacc2).ToList();
            messages = messages.OrderByDescending(m => m.MessageDate).ToList();
            return messages;
        }

        public void SendMes(Account accountInDb, int dialId, string text)
        {
            var message = new Message();
            message.DialogId = dialId;
            message.MessageDate = DateTime.Now;
            message.Text = text;
            message.AccountId = accountInDb.Id;
            _context.Messages.Add(message);
            _context.SaveChanges();
        }
    }
}
