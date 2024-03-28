using APP.KMX.Entities;
using Microsoft.AspNetCore.Mvc;

namespace APP.KMX.Services.Interfaces
{
    public interface IFileService
    {
        public Task<string> ConvertFileAsync(IFormFile fileData, string uploadPath);



    }
}
