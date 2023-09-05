using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    public class PhysiotherapistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
