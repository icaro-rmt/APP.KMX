using APP.KMX.Models;
using APP.KMX.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace APP.KMX.Controllers
{
    public class FileController: Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
      

        public FileController(IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
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

        [HttpGet]
        public ActionResult DownloadDocument(string file)
        {
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads") + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/force-download", file);

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
