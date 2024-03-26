using APP.KMX.Entities;
using APP.KMX.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APP.KMX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController: ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
            
        }

        public async Task<ActionResult> PostSingleFile([FromForm] FileUploadModel fileDetails)
        {
            if(fileDetails == null)
            {
                return BadRequest();

            }
            try
            {
                await _fileService.PostFileAsync(fileDetails.FileDetails, fileDetails.FileType);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }

        }
 

    }
}
