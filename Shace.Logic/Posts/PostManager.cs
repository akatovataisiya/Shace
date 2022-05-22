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

        public void DeletePost(int postid)
        {
            var post = _context.Posts.FirstOrDefault(p=> p.Id==postid);
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }

        public void UpdatePost(string? description, string? location, int postid)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == postid);
            if(description!=null)
                post.Description = description;
            if(location!=null)
                post.Location = location;
            _context.SaveChanges();
        }

        public Post GetPost(int postid)
        {
            return _context.Posts.FirstOrDefault(post => post.Id == postid);
        }

        public List<Comment> GetComments(int postid)
        {
            return _context.Comments.Where(comment => comment.PostId == postid).ToList();
        }

        public Account GetAcc(int id)
        { 
            return _context.Accounts.FirstOrDefault(account => account.Id == id);
        }

        public List<Account> GetAccs()
        { 
            return _context.Accounts.ToList();
        }

        public List<Like> GetLikes(int postid)
        { 
            return _context.Likes.Where(like => like.PostId == postid).ToList();
        }

        public void Like(int postid, int accountid)
        {

            Like _like = new Like();
            var postInDb = _context.Posts.Where(post => post.Id == postid).FirstOrDefault();
            _like.PostId = postid;
            postInDb.LikeCounter += 1;
            _like.AccountId = accountid;
            _context.Likes.Add(_like);
            _context.SaveChanges();
        }

        public void DeleteLike(int postid, int accountid)
        {
            Like _like = new Like();
            var postInDb = _context.Posts.Where(post => post.Id == postid).FirstOrDefault();
            var alreadyLiked = _context.Likes.FirstOrDefault(liker => liker.PostId == postid && liker.AccountId == accountid);
            postInDb.LikeCounter -= 1;
            _context.Likes.Remove(alreadyLiked);
            _context.SaveChanges();
        }

        public void CommentPost(Comment _comm, int postid)
        {
            var postInDb = _context.Posts.Where(post => post.Id == postid).FirstOrDefault();
            postInDb.CommentCounter += 1;
            _context.Comments.Add(_comm);
            _context.SaveChanges();
        }
    }
}
