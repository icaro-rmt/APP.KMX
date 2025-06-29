using APP.KMX.App.Models;
using APP.KMX.App.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace APP.KMX.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(PasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Proceed with registration or any other logic
                return RedirectToAction("Success");
            }

            // If model validation failed, return the same view to show errors
            return View(model);
        }
        public IActionResult RegisterPage()
        {
            return View();
        }
        public IActionResult FileConversion()
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
