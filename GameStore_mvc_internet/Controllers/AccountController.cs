using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
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
        SqlConnection con;
        string serverConnection = "Data Source=morkva;Initial Catalog=GameStore;Integrated Security=True";

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

                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", game.Name);

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

                TempData["message"] = string.Format("Изменения в акции \"{0}\" были сохранены", share.Name);
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

                TempData["message"] = string.Format("Изменения в новости \"{0}\" были сохранены", news.Name);
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
                TempData["message"] = string.Format("Акция \"{0}\" была удалена",
                    dbEntry.Name);
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
                TempData["message"] = string.Format("Новость \"{0}\" была удалена",
                    dbEntry.Name);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int gameId)
        {
            serverConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Game dbEntry = db.Games.Find(gameId);
            if (dbEntry != null)
            {
                db.Games.Remove(dbEntry);
                db.SaveChanges();
                con = new SqlConnection(serverConnection);
                string comDelete = string.Format("DELETE from Comments WHERE  ShopId = @shopid;");
                con.Open();
                SqlCommand com = new SqlCommand(comDelete, con);
                com.Parameters.AddWithValue("shopid", gameId);
                com.ExecuteNonQuery();
                con.Close();

            }

            if (dbEntry != null)
            {
                TempData["message"] = string.Format("Игра \"{0}\" была удалена",
                    dbEntry.Name);
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
                    TempData["message"] = string.Format("Слайд \"{0}\" был удален",
                  photoName);
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
                string pic = System.IO.Path.GetFileName(image.FileName);
                string path = System.IO.Path.Combine(Server.MapPath(@"~/Images/Slider"), pic);


                image.SaveAs(path);
                TempData["message"] = string.Format("Слайд \"{0}\" был добавлен",
                 pic);
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
        public ActionResult Login(string LoginTextBox, string PasswordTextBox)
        {
            if (User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index");

            }
            if (LoginTextBox != "" && PasswordTextBox != "")
            {
                if (ModelState.IsValid && WebSecurity.Login(LoginTextBox, PasswordTextBox, persistCookie: true))
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
