using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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