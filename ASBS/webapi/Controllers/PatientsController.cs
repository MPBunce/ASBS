using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    public class PatientsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
