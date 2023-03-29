using Microsoft.AspNetCore.Mvc;

namespace YourProject.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}