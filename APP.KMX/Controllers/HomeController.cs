using APP.KMX.Models;
using APP.KMX.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace APP.KMX.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        private readonly string _uploadFolder;

        private string filePath = string.Empty;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            _uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        }
      

        public IActionResult Index()
        {
            var cards = GenerateCards();
            return View(cards);
        }

        [HttpPost]
        public async Task<ActionResult> Index(IFormFile file)
        {
            try
            { 
                var uploadedFile = await _fileService.ConvertFileAsync(file, _uploadFolder);
                filePath = uploadedFile;
                string fileName = Path.GetFileName(filePath);

                return DownloadDocument(fileName);
                
            }
            catch (Exception)
            {
                throw new Exception();
            }
            
        }
        [HttpPost]
        public async Task<ActionResult> ConverterKml(IFormFile file)
        {
            try
            {
                var uploadedFile = await _fileService.ConvertFileAsync(file, _uploadFolder);
                filePath = uploadedFile;
                string fileName = Path.GetFileName(filePath);

                return DownloadDocument(fileName);
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        [HttpPost]
        public async Task<ActionResult> ConverterExcel(IFormFile file)
        {
            try
            {
                var uploadedFile = await _fileService.ConvertFileAsync(file, _uploadFolder);
                filePath = uploadedFile;
                string fileName = Path.GetFileName(filePath);

                return DownloadDocument(fileName);

            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        public ActionResult DownloadSampleDocument()
        {
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "fileModels");
            filePath = uploadFolder + "/modeloExcel.xlsx";
            try
            {
                if (filePath != string.Empty)
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return File(fileBytes, "application/force-download", "modeloExcel.xlsx");

                }
                return View(Index());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        protected private List<CardModel> GenerateCards()
        {
            var cards = new List<CardModel>
            {
                new CardModel
                {
                    ImageUrl = Url.Content("~/img/xls-kml.png"),
                    Title = "Converter Excel",
                    Text = "Converter de Excel para KML",
                    ActionName = "ConverterExcel",
                    ControllerName = "Home",
                    ShowForm = true,
                },
                new CardModel
                {
                    ImageUrl = Url.Content("~/img/kml-xls.png"),
                    Title = "Converter KML",
                    Text = "Converter de KML para Excel",
                    ActionName = "ConverterKml",
                    ControllerName = "Home",
                    ShowForm = true,
                },
                new CardModel
                {
                    ImageUrl = Url.Content("~/img/microsoft-excel.png"),
                    Title = "Modelo de Excel",
                    Text = "Download Arquivo Modelo Excel",
                    ActionName = "DownloadSampleDocument",
                    ControllerName = "Home",
                    ShowForm = false,
                },
            };
            return cards;
        }
    }
}