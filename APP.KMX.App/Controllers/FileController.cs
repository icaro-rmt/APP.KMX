using APP.KMX.Services;
using APP.KMX.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APP.KMX.App.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadFolder;
        private readonly IFileService _fileService;

        private string filePath = string.Empty;
        
        public FileController(IWebHostEnvironment webHostEnvironment, IFileService fileService )
        {
            _webHostEnvironment = webHostEnvironment;
            _uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            _fileService = fileService;

        }
        public IActionResult Index()
        {
            var model = "" ;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file Uploaded");
            }

            return await this.ConverterKml(file);
        }

        [HttpPost]
        public async Task<ActionResult> ConverterKml(IFormFile file)
        {
            try
            {
                var uploadedFile = await _fileService.ConvertFileAsync(file, _uploadFolder);
                filePath = uploadedFile;
                string fileName = Path.GetFileName(filePath);

                return await DownloadDocument(fileName);
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

                return await DownloadDocument(fileName);

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

                    return File(fileBytes, "application/octet-stream", "modeloExcel.xlsx");

                }
                return View(Index());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<ActionResult >DownloadDocument(string fileName)
        {
            try
            {
                if (filePath != string.Empty)
                {
                    var mimeType = "application/octet-stream"; // Default, use more specific type if known
                    if (filePath.EndsWith(".pdf"))
                        mimeType = "application/pdf";
                    else if (filePath.EndsWith(".xls"))
                        mimeType = "application/vnd.ms-excel";
                    else if (filePath.EndsWith(".xlsx"))
                        mimeType = "application/vnd.ms-excel";
                    else if (filePath.EndsWith(".kml"))
                        mimeType = "application/vnd.google-earth.kml+xml";
                    else if (filePath.EndsWith(".kmz"))
                        mimeType = "application/vnd.google-earth.kmz";

                    byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                    return File(fileBytes, mimeType, fileName);

                }
                return View(Index());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
