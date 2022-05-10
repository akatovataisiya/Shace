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
