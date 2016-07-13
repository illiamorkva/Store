using GameStore_mvc_internet.Filters;
using GameStore_mvc_internet.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GameStore_mvc_internet.Controllers
{
    [Culture]
    public class CartController : Controller
    {
        private GameContext db = new GameContext();

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingInfo)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }
            if (shippingInfo.Captcha != (string)Session["code"])
            {
                ModelState.AddModelError("Captcha", "Текст с картинки введен неверно");
            }
            if (ModelState.IsValid)
            {
                // Получение данных из файла web.config
                string server = ConfigurationManager.AppSettings["server"];
                string port = ConfigurationManager.AppSettings["port"];
                string login = ConfigurationManager.AppSettings["login"];
                string password = ConfigurationManager.AppSettings["password"];

                // Настройка объекта сообщения
                MailMessage message = new MailMessage();
                message.Subject = "Новый заказ отправлен";
                message.From = new MailAddress(login);
                message.To.Add(new MailAddress("adress@yandex.ua"));

                StringBuilder body = new StringBuilder()
                    .AppendLine("Новый заказ обработан")
                    .AppendLine("---")
                    .AppendLine("Товары:");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Game.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (итого: {2:c}",
                        line.Quantity, line.Game.Name, subtotal);
                }
                body.AppendFormat("Общая стоимость: {0:c}", cart.ComputeTotalValue())
                   .AppendLine("---")
                   .AppendLine("Доставка:")
                   .AppendLine(shippingInfo.Name)
                   .AppendLine(shippingInfo.Line1)
                   .AppendLine(shippingInfo.Line2 ?? "")
                   .AppendLine(shippingInfo.Line3 ?? "")
                   .AppendLine(shippingInfo.City)
                   .AppendLine(shippingInfo.Country)
                   .AppendLine("---")
                   .AppendFormat("Подарочная упаковка: {0}",
                       shippingInfo.GiftWrap ? "Да" : "Нет");
                message.Body = body.ToString();


                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;

                // Настройка клиента для подключения к серверу
                SmtpClient client = new SmtpClient();
                client.Host = server;
                client.Port = Convert.ToInt32(port);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(login, password);

                // Отправка сообщения.
                client.Send(message);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingInfo);
            }
        }

        public ActionResult Captcha()
        {
            string code = new Random(DateTime.Now.Millisecond).Next(11111, 99999).ToString();
            Session["code"] = code;
            CaptchaImage captcha = new CaptchaImage(code, 110, 50);

            Response.Clear();
            Response.ContentType = "image/jpeg";

            captcha.Image.Save(Response.OutputStream, ImageFormat.Jpeg);

            captcha.Dispose();
            return null;
        }

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer?.AbsolutePath;

            List<string> cultures = new List<string>() { "ru", "en", "uk" };
            if (!cultures.Contains(lang))
            {
                lang = "uk";
            }
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

        // итоговый подсчет
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        public ViewResult Index(Cart cart, string returnUrl) // В параметре cart, связка, из сессии берем обьект
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        // добавления в корзину
        public RedirectToRouteResult AddToCart(Cart cart, int gameId, string returnUrl)
        {
            Game game = db.Games
                .FirstOrDefault(g => g.GameId == gameId);

            if (game != null)
            {
                cart.AddItem(game, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        //удаления из корзины
        public RedirectToRouteResult RemoveFromCart(Cart cart, int gameId, string returnUrl)
        {
            Game game = db.Games
                .FirstOrDefault(g => g.GameId == gameId);

            if (game != null)
            {
                cart.RemoveLine(game);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        //получения обьекта из сессии
        // есть связыв. binder
        /* public Cart GetCart()
         {
             Cart cart = (Cart)Session["Cart"];
             if (cart == null)
             {
                 cart = new Cart();
                 Session["Cart"] = cart;
             }
             return cart;
         }
         */
    }
}
