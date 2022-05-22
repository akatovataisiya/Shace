using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shace.Logic.Posts
{
    public interface IPostManager
    {
        public void UpdatePost(string? description, string? location, int postid);
        public void DeletePost(int postid);
        public Post GetPost(int postid);
        public List<Comment> GetComments(int postid);
        public Account GetAcc(int id);
        public List<Account> GetAccs();
        public List<Like> GetLikes(int postid);
        public void Like(int postid, int accountid);
        public void DeleteLike(int postid, int accountid);
        public void CommentPost(Comment _comm, int postid);
    }
}
