using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore_mvc_internet.Models
{
    public class SearchGamesListViewModel
    {
        public IEnumerable<Game> Games { get; set; } // игры
        public PagingInfo PagingInfo { get; set; } // отображение товаров
        public string Term { get; set; } // строка запроса
    }
}