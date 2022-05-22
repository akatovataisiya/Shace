namespace Shace.Logic.ProfilePage
{
    public interface IProfilePageManager
    {
        public Account GetAcc(string shortName);
        public List<Post> GetPosts(Account account);
        public Subscribtion IfSub(Account accountInDb, Account accountInDbUser);
        public void Subscribe(Account accountInDb, Account accountInDbUser);
        public void CreateAd(Advertisment _ad);
         public Dialog IfDial(Account accountInDb, Account accountInDbUser);
        public int Dialog(Account accountInDb, Account accountInDbUser);
        public List<Account> GetDial(Account accountInDb);
        public List<Message> GetMes(Account accountInDb, Account accountInDbUser, int dialId);
        public void SendMes(Account accountInDb, int dialId, string text);
    }
}
