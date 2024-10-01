using Microsoft.AspNetCore.Mvc;

namespace PBL_LP.Controllers
{
    public class Dashboard1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
