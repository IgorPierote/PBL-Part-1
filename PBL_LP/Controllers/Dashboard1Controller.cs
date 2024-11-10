using Microsoft.AspNetCore.Mvc;
using PBL_LP.Filters;

namespace PBL_LP.Controllers
{
    [ServiceFilter(typeof(AutorizacaoFilter))]
    public class Dashboard1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
