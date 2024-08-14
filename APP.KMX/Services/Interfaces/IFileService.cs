namespace APP.KMX.Services.Interfaces
{
    public interface IFileService
    {
        public Task<string> ConvertFileAsync(IFormFile fileData, string uploadPath);
        public Task ClearFiles(string uploadDirectory);
    }
}
