using APP.KMX.Entities;
using APP.KMX.Models;
using APP.KMX.Services.Interfaces;
using APP.KMX.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace APP.KMX.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            await _fileService.ConvertFileAsync(file, uploadFolder);


            return View();
        }

        public ActionResult DownloadDocument(string filePath, string fileName)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/force-download", fileName);

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