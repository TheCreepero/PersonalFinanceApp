using Microsoft.AspNetCore.Mvc;
using Site.Data;
using Site.Models;
using Site.Utility;
using System.Diagnostics;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly AccountService _accountService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, AccountService accountService)
        {
            _logger = logger;
            _context = context;
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            _accountService.CalculateAccountBalance();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}