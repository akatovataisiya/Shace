namespace Shace.Logic.ProfilePage
{
    public interface IProfilePageManager
    {
        public Account GetAcc(string shortName);
        public List<Post> GetPosts(Account account);
        public Subscribtion IfSub(Account accountInDb, Account accountInDbUser);
        public void Subscribe(Account accountInDb, Account accountInDbUser);
    }
}
