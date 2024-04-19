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

        private string filePath = string.Empty;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(IFormFile file)
        {
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            try
            {
                var uploadedFile = await _fileService.ConvertFileAsync(file, uploadFolder);
                filePath = uploadedFile;
                string fileName = Path.GetFileName(filePath);

                return DownloadDocument(fileName);
            }
            catch (Exception)
            {

                return BadRequest("Arquivo com erro") ;
            }
            
        }

        public ActionResult DownloadDocument(string fileName)
        {
            try
            {
                if (filePath != string.Empty)
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return File(fileBytes, "application/force-download", fileName);

                }
                return View(Index());
            }
            catch (Exception)
            {

                return View(Index());
            }

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