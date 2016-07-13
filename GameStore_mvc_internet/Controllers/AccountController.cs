using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using GameStore_mvc_internet.Filters;
using GameStore_mvc_internet.Models;
using System.IO;
using System.Data.SqlClient;

namespace GameStore_mvc_internet.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private SqlConnection _con;
        private string _serverConnection = "Data Source=morkva;Initial Catalog=GameStore;Integrated Security=True";

        private GameContext db = new GameContext();
        private NewsContext dbNews = new NewsContext();
        private ShareContext dbShare = new ShareContext();

        public ViewResult Index()
        {
            NewsListGameViewModel model = new NewsListGameViewModel
            {
                Games = db.Games,
                News = dbNews.News,
                Shares = dbShare.Shares

            };
            return View(model);
        }

        public ViewResult Edit(int gameId)
        {
            Game game = db.Games
                            .FirstOrDefault(g => g.GameId == gameId);
            if (game != null)
            {
                return View(game);
            }
            else
            {
                NewsListGameViewModel model = new NewsListGameViewModel
                {
                    Games = db.Games,
                    News = dbNews.News,
                    Shares = dbShare.Shares

                };
                return View("Index", model);
            }

        }


        public ViewResult EditShare(int shareId)
        {
            Share share = dbShare.Shares
                           .FirstOrDefault(g => g.ShareId == shareId);
            if (share != null)
            {
                return View(share);
            }
            else
            {
                NewsListGameViewModel model = new NewsListGameViewModel // новости и акции
                {
                    Games = db.Games,
                    News = dbNews.News,
                    Shares = dbShare.Shares

                };
                return View("Index", model);
            }

        }


        public ViewResult EditNews(int newsId)
        {
            News news = dbNews.News
                .FirstOrDefault(g => g.NewsId == newsId);
            if (news != null)
            {
                return View(news);
            }
            else
            {
                NewsListGameViewModel model = new NewsListGameViewModel
                {
                    Games = db.Games,
                    News = dbNews.News,
                    Shares = dbShare.Shares

                };
                return View("Index", model);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Game game, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    game.ImageMimeType = image.ContentType;
                    game.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(game.ImageData, 0, image.ContentLength);
                }

                if (game.GameId == 0)
                    db.Games.Add(game);
                else
                {
                    Game dbEntry = db.Games.Find(game.GameId);
                    if (dbEntry != null)
                    {
                        dbEntry.Name = game.Name;
                        dbEntry.Description = game.Description;
                        dbEntry.Price = game.Price;
                        dbEntry.Category = game.Category;
                        dbEntry.ImageData = game.ImageData;
                        dbEntry.ImageMimeType = game.ImageMimeType;
                    }
                }
                db.SaveChanges();

                TempData["message"] = $"Изменения в игре \"{game.Name}\" были сохранены";

                return RedirectToAction("Index");
            }
            else
            {
                return View(game);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditShare(Share share)
        {
            if (ModelState.IsValid)
            {
                if (share.ShareId == 0)
                {
                    share.Date = DateTime.Now;
                    dbShare.Shares.Add(share);
                }
                else
                {
                    Share dbEntry = dbShare.Shares.Find(share.ShareId);
                    if (dbEntry != null)
                    {
                        dbEntry.Name = share.Name;
                        dbEntry.Description = share.Description;
                        dbEntry.Date = DateTime.Now;

                    }
                }
                dbShare.SaveChanges();

                TempData["message"] = $"Изменения в акции \"{share.Name}\" были сохранены";
                return RedirectToAction("Index");
            }
            else
            {
                return View(share);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNews(News news)
        {
            if (ModelState.IsValid)
            {
                if (news.NewsId == 0)
                {
                    news.Date = DateTime.Now;
                    dbNews.News.Add(news);
                }
                else
                {
                    News dbEntry = dbNews.News.Find(news.NewsId);
                    if (dbEntry != null)
                    {
                        dbEntry.Name = news.Name;
                        dbEntry.Description = news.Description;
                        dbEntry.Date = DateTime.Now;

                    }
                }
                dbNews.SaveChanges();

                TempData["message"] = $"Изменения в новости \"{news.Name}\" были сохранены";
                return RedirectToAction("Index");
            }
            else
            {
                return View(news);
            }
        }
        public ViewResult CreateShare()
        {
            return View("EditShare", new Share());
        }
        public ViewResult CreateNews()
        {
            return View("EditNews", new News());
        }
        public ViewResult Create()
        {
            return View("Edit", new Game());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteShare(int shareId)
        {
            Share dbEntry = dbShare.Shares.Find(shareId);
            if (dbEntry != null)
            {
                dbShare.Shares.Remove(dbEntry);
                dbShare.SaveChanges();
            }

            if (dbEntry != null)
            {
                TempData["message"] = $"Акция \"{dbEntry.Name}\" была удалена";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteNews(int newsId)
        {
            News dbEntry = dbNews.News.Find(newsId);
            if (dbEntry != null)
            {
                dbNews.News.Remove(dbEntry);
                dbNews.SaveChanges();
            }

            if (dbEntry != null)
            {
                TempData["message"] = $"Новость \"{dbEntry.Name}\" была удалена";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int gameId)
        {
            _serverConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Game dbEntry = db.Games.Find(gameId);
            if (dbEntry != null)
            {
                db.Games.Remove(dbEntry);
                db.SaveChanges();
                _con = new SqlConnection(_serverConnection);
                string comDelete = "DELETE from Comments WHERE  ShopId = @shopid;";
                _con.Open();
                SqlCommand com = new SqlCommand(comDelete, _con);
                com.Parameters.AddWithValue("shopid", gameId);
                com.ExecuteNonQuery();
                _con.Close();

            }

            if (dbEntry != null)
            {
                TempData["message"] = $"Игра \"{dbEntry.Name}\" была удалена";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePhotoSlider(string photoName)
        {
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath(@"~/Images/Slider"));
            var files = dir.GetFiles();
            foreach (var c in files)
            {
                if (c.Name == photoName)
                {
                    c.Delete();
                    TempData["message"] = $"Слайд \"{photoName}\" был удален";
                }
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreatePhotoSlider(HttpPostedFileBase image = null)
        {
            if (image != null)
            {
                string pic = Path.GetFileName(image.FileName);
                if (pic != null)
                {
                    string path = Path.Combine(Server.MapPath(@"~/Images/Slider"), pic);


                    image.SaveAs(path);
                }
                TempData["message"] = $"Слайд \"{pic}\" был добавлен";
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            Response.Cache.SetExpires(DateTime.Now.AddMonths(-1));
            Response.Cache.SetCacheability(HttpCacheability.Server);
            Response.Cache.SetNoStore();

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");

            }

            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string loginTextBox, string passwordTextBox)
        {
            if (User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index");

            }
            if (loginTextBox != "" && passwordTextBox != "")
            {
                if (ModelState.IsValid && WebSecurity.Login(loginTextBox, passwordTextBox, persistCookie: true))
                {
                    return RedirectToAction("Index", "Account");
                }
            }
            return View();


        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }
    }
}
