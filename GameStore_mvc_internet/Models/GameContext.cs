using System.Data.Entity;

namespace GameStore_mvc_internet.Models
{
    public class GameContext : DbContext
    {
         public GameContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Game> Games { get; set; }
    }
}