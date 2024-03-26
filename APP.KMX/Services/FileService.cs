using APP.KMX.Entities;
using APP.KMX.Services.Interfaces;

namespace APP.KMX.Services
{
    public class FileService : IFileService
    {
        public async Task PostFileAsync(IFormFile fileData, FileType fileType)
        {
            try
            {
                var fileDetails = new FileDetails()
                {
                    Id = 0,
                    FileName = fileData.FileName,
                    FileType = fileType,
                };
                
                using(var stream = new MemoryStream())
                {
                    fileData.CopyTo(stream);
                    fileDetails.FileData = stream.ToArray();
                }
                Console.WriteLine("Resultado: ", fileData);

            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }
    }
}
