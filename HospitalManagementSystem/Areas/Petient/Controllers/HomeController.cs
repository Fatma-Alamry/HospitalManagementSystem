using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Areas.Petient.Controllers
{
    [Area("Petient")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
