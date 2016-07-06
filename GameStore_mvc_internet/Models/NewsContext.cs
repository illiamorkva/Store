using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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