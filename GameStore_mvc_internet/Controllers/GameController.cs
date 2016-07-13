using GameStore_mvc_internet.Models;
using System.Linq;
using System.Web.Mvc;

namespace GameStore_mvc_internet.Controllers
{
    public class GameController : Controller
    {
        private GameContext db = new GameContext();
        public FileContentResult GetImage(int gameId)
        {
            Game game = db.Games
                .FirstOrDefault(g => g.GameId == gameId);

            if (game != null)
            {
                return File(game.ImageData, game.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

    }
}
