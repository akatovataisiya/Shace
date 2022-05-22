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
    }
}
