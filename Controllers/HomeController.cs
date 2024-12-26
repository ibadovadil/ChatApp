using Microsoft.AspNetCore.Mvc;

namespace WebSocket.Controllers
{
    public class HomeController : Controller
    {
      

        public IActionResult Index()
        {
            return View();
        }

    }
}
