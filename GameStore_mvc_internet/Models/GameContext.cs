using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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