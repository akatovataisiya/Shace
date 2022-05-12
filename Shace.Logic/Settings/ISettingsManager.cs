namespace Shace.Logic.Settings
{
    public interface ISettingsManager
    {
        public void ChangeAccount(string email, string shortName, long phone, string description, string photo, string location, DateTime bDay, Account account);

        public void ChangePassword(string password, Account account);

        public void Delete(int id);

        public void PrivacyTrueorFalse(bool privacy, Account account);
        public bool FindEmail(string email);
        public bool FindShortName(string shortName);
    }
}
