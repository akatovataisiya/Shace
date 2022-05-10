using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shace.Logic.Posts
{
    public class PostManager : IPostManager
    {
        private readonly AccountContext _context;

        public PostManager(AccountContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPost()
        {
            /*var accountInDb = _context.Accounts.FirstOrDefault(acc => (acc.Email == email || acc.ShortName == shortName));
            if (accountInDb == null)
            {
                var account = new Account();
                account.Email = email;
                account.ShortName = shortName;
                account.Password = password;
                account.RegDay = DateTime.Now;
                account.Url = $"https://shace.com/{shortName}";
                account.Privacy = false;
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();*/
                return true;
        }

        public Task<bool> DeleteAllPost()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePost()
        {
            throw new NotImplementedException();
        }

        public Post GetPost(int id)
        {
            var postInDb = _context.Posts.FirstOrDefault(post => post.AccountId == id);
            if (postInDb != null)
                return postInDb;
            else return null;
        }

        public Post GetPost()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePost()
        {
            throw new NotImplementedException();
        }
    }
}
