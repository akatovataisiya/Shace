using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shace.Logic.Posts
{
    internal interface IPostManager
    {
        public Task<bool> AddPost();
        public Task<bool> UpdatePost();
        public Task<bool> DeletePost();
        public Task<bool> DeleteAllPost();
        public Post GetPost();
    }
}
