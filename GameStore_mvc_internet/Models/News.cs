using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore_mvc_internet.Models
{
    public class News
    {
        [HiddenInput(DisplayValue = false)]
        public int NewsId { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название новости")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание новости")]
        public string Description { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime Date { get; set; }
    }
}