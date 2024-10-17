using Microsoft.AspNetCore.Mvc;

namespace APP.KMX.App.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadFolder;

        private string filePath = string.Empty;
        
        public FileController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

        }
        public IActionResult Index()
        {
            var model = "" ;
            return View(model);
        }
    }
}
