using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore_mvc_internet.Models
{
    public class Share
    {
        [HiddenInput(DisplayValue = false)]
        public int ShareId { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название акции")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание акции")]
        public string Description { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime Date { get; set; }
    }
}