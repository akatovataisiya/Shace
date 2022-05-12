namespace Shace.Shace.Logic.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private readonly AccountContext _context;

        public SettingsManager(AccountContext context)
        {
            _context = context;
        }
        public void ChangeAccount(string email, string shortName, long phone, string description, string photo, string location, DateTime bDay, Account account)
        {
            if (bDay != new DateTime(1, 1, 1))
                account.BDay = bDay;
            if (photo != null)
                account.Photo = photo;
            if (location != null)
                account.Location = location;
            if (description != null)
                account.Description = description;
            if (phone != 0)
                account.Phone = phone;
            if (email != null)
            {
                var em = FindEmail(email);
                if (!em)
                    account.Email = email;
            }
            if (shortName != null)
            {
                var pass = FindShortName(shortName);
                if (!pass)
                {
                    account.ShortName = shortName;
                    account.Url = $"https://shace.com/{account.ShortName}";
                }
            }
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

        public void ChangePassword(string password, Account account)
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
                _context.Accounts.Remove(account);
                _context.SaveChanges();
            }

        }

    }
}
