using System.Data.Entity;

namespace GameStore_mvc_internet.Models
{
    public class ShareContext : DbContext
    {
        public ShareContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Share> Shares { get; set; }
    }
}