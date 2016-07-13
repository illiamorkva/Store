using System.Collections.Generic;

namespace GameStore_mvc_internet.Models
{
    public class CommentsListGameViewModel
    {
        public Game Game { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}