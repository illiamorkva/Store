using System.Data.Entity;

namespace GameStore_mvc_internet.Models
{
    public class NewsContext : DbContext
    {
         public NewsContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<News> News { get; set; }
    }
}