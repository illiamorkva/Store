using System.Collections.Generic;

namespace GameStore_mvc_internet.Models
{
    public class SearchGamesListViewModel
    {
        public IEnumerable<Game> Games { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string Term { get; set; }
    }
}