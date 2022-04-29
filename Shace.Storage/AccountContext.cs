using Microsoft.EntityFrameworkCore;
using Shace.Storage.Entities;

namespace Shace.Storage
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Mark> Marks { get; set; }

        public DbSet<Subscribtion> Subscribtions { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Dialog> Dialogs { get; set; }

        public DbSet<Advertisment> Advertisments { get; set; }

        public DbSet<Account> Accounts { get; set; }
    }
}
