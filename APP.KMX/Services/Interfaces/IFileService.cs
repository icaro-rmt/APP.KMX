using APP.KMX.Entities;

namespace APP.KMX.Services.Interfaces
{
    public interface IFileService
    {
        public Task ConvertFileAsync(IFormFile fileData, string uploadPath);

    }
}
