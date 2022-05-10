namespace Shace.Logic.Accounts
{
    public interface IAccountManager
    {
        public Task<bool> SignIn(string email, string password);
        public Task<bool> Create(string email, string shortName, string password);
        public Task<bool> Find(string email);
        public Account GetAccByEmail(string email);
    }
}
