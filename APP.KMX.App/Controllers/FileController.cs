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
        private readonly ILogger<FileController> _logger;
        
        public FileController(IWebHostEnvironment webHostEnvironment, IFileService fileService, ILogger<FileController> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            _fileService = fileService;
            _logger = logger;
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
                _logger.LogWarning("Upload attempted with no file");
                return BadRequest("No file uploaded");
            }

            return await ConvertFile(file);
        }

        [HttpPost]
        public async Task<ActionResult> ConvertFile(IFormFile file)
        {
            try
            {
                _logger.LogInformation("Starting file conversion for: {FileName}", file.FileName);
                
                var convertedFilePath = await _fileService.ConvertFileAsync(file, _uploadFolder);
                
                if (string.IsNullOrEmpty(convertedFilePath))
                {
                    _logger.LogError("File conversion failed - no output file generated");
                    return StatusCode(500, "File conversion failed");
                }

                var fileName = Path.GetFileName(convertedFilePath);
                return await DownloadDocument(convertedFilePath, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting file: {FileName}", file.FileName);
                return StatusCode(500, "An error occurred during file conversion");
            }
        }

        public ActionResult DownloadSampleDocument()
        {
            try
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "fileModels");
                string sampleFilePath = Path.Combine(uploadFolder, "modeloExcel.xlsx");
                
                if (!System.IO.File.Exists(sampleFilePath))
                {
                    _logger.LogWarning("Sample file not found at: {Path}", sampleFilePath);
                    return NotFound("Sample file not available");
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(sampleFilePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "modeloExcel.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading sample document");
                return StatusCode(500, "Error downloading sample file");
            }
        }

        public async Task<ActionResult> DownloadDocument(string filePath, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                {
                    _logger.LogWarning("File not found: {FilePath}", filePath);
                    return NotFound("File not found");
                }

                var mimeType = GetMimeType(filePath);
                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                // Clean up the file after reading
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete temporary file: {FilePath}", filePath);
                }

                return File(fileBytes, mimeType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading document: {FilePath}", filePath);
                return StatusCode(500, "Error downloading file");
            }
        }

        private static string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".kml" => "application/vnd.google-earth.kml+xml",
                ".kmz" => "application/vnd.google-earth.kmz",
                _ => "application/octet-stream"
            };
        }

    }
}
