using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI;

namespace GameStore_mvc_internet.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ErrorController : Controller
    {
        // 500
        public ActionResult InternalServerError()
        {
            SetResponse(HttpStatusCode.InternalServerError);
            return View();
        }

        // 404
        public ActionResult NotFound()
        {
            SetResponse(HttpStatusCode.NotFound);
            return View();
        }

        // 403
        public ActionResult Forbidden()
        {
            SetResponse(HttpStatusCode.Forbidden);
            return View();
        }

        private void SetResponse(HttpStatusCode httpStatusCode)
        {
            Response.StatusCode = (int)httpStatusCode;
            Response.StatusDescription = HttpWorkerRequest.GetStatusDescription((int)httpStatusCode);
        }

        protected override void OnActionExecuted(ActionExecutedContext context)
        {
            var response = context.HttpContext.Response;
            using (StreamWriter file = new StreamWriter(Server.MapPath(@"~/Errors/log.txt"), true))
            {
                file.WriteLine(
                    $"Ошибка {response.StatusCode} с описанием \"{response.StatusDescription}\" произошла в {DateTime.Now}");

            }
        }
    }
}
