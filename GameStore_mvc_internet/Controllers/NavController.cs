using GameStore_mvc_internet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore_mvc_internet.Controllers
{
    // формирует меню категорий
    public class NavController : Controller
    {
        private GameContext db = new GameContext();
        public PartialViewResult Menu(string category = null)                  
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = db.Games
                .Select(game => game.Category)
                .Distinct()
                .OrderBy(x => x);
          return PartialView("FlexMenu", categories);
        }

    }
}
