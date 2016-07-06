using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore_mvc_internet.Models
{
    public class NewsListGameViewModel
    {
        public IEnumerable<News> News { get; set; }
         public IEnumerable<Share> Shares { get; set; }
         public IEnumerable<Game> Games { get; set; }
    }
}