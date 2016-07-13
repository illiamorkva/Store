using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore_mvc_internet.Models
{
    public class Comment
    {
        [HiddenInput(DisplayValue = false)]
        public int CommentId { get; set; }
        public int ShopId { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}