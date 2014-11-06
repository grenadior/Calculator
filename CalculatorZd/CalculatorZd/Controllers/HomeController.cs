using System.Web.Mvc;
using CalculatorZd.Models;

namespace CalculatorZd.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new EmailActivationSendModels());
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "О Нас.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Контакты.";

            return View();
        }
    }
}