using GameStore_mvc_internet.Filters;
using GameStore_mvc_internet.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GameStore_mvc_internet.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        private GameContext db = new GameContext();
        private NewsContext dbNews = new NewsContext();
        private ShareContext dbShare = new ShareContext();
        private CommentContext dbComment = new CommentContext();

        public int PageSize = 4;

        public ViewResult List(string category, int page = 1)
        {
            GamesListViewModel model = new GamesListViewModel
            {
                Games = db.Games
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(game => game.GameId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                    db.Games.Count() :
                    db.Games.Where(game => game.Category == category).Count()
                },

                CurrentCategory = category
            };
            return View(model);

        }

        public ViewResult Index()
        {
            NewsListGameViewModel model = new NewsListGameViewModel // новости и акции
            {
                Games = null,
                News = dbNews.News
                .OrderByDescending(news => news.NewsId)
                .Take(3),
                Shares = dbShare.Shares
                .OrderByDescending(shares => shares.ShareId)
                .Take(3)

            };
            return View(model);
        }

        public ViewResult Details(int gameId)
        {
            Game game = db.Games
                .FirstOrDefault(g => g.GameId == gameId);
            if (game != null)
            {
                CommentsListGameViewModel model = new CommentsListGameViewModel
                {
                    Game = game,
                    Comments = dbComment.Comments
                     .Where(p => p.ShopId == gameId)
                };
                return View(model);
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
        public ActionResult AddComment(string author, string comment, int gameId, string returnUrl)
        {
            Comment com = new Comment();
            com.Author = author;
            com.ShopId = gameId;
            com.Description = comment;
            com.Date = DateTime.Now;
            dbComment.Comments.Add(com);
            dbComment.SaveChanges();

            if (Request.IsAjaxRequest())
            {
                return PartialView(com);
            }
            return Redirect(returnUrl);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteComment(int commentId, string returnUrl)
        {
            Comment dbEntry = dbComment.Comments.Find(commentId);
            if (dbEntry != null)
            {
                dbComment.Comments.Remove(dbEntry);
                dbComment.SaveChanges();
            }
            return Redirect(returnUrl);
        }
    }
}
