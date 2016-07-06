using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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