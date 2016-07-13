using System.Data.Entity;

namespace GameStore_mvc_internet.Models
{
    public class CommentContext : DbContext
    {
        public CommentContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Comment> Comments { get; set; }
    }
}