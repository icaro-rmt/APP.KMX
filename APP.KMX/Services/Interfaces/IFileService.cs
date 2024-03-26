using APP.KMX.Entities;

namespace APP.KMX.Services.Interfaces
{
    public interface IFileService
    {
        public Task PostFileAsync(IFormFile fileData, FileType fileType);

    }
}
