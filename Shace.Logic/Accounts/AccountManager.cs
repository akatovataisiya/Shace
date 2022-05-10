using Shace.Storage.Entities;

namespace Shace.Logic.Accounts
{
    public class AccountManager : IAccountManager
    {
        private readonly AccountContext _context;

        public AccountManager(AccountContext context)
        {
            _context = context;
        }


        public async Task<bool> SignIn(string email, string password)
        {
            var accountInDb = _context.Accounts.FirstOrDefault(st => st.Email == email && st.Password == password);
            await Task.Delay(0);
            if (accountInDb != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Find(string email)
        {
            var accountInDb = _context.Accounts.FirstOrDefault(st => (st.Email == email));
            await Task.Delay(0);
            if (accountInDb == null)
                return false;
            return true;
        }

        public async Task<bool> Create(string email, string shortName, string password)
        {
            var accountInDb = _context.Accounts.FirstOrDefault(st => (st.Email == email || st.ShortName == shortName));
            if (accountInDb == null)
            {
                var account = new Account();
                account.Photo = "prof.png";
                account.Email = email;
                account.ShortName = shortName;
                account.Password = password;
                account.RegDay = DateTime.Now;
                account.Url = $"https://shace.com/{shortName}";
                account.Privacy = false;
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task Delete(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }

        }


    }
}
