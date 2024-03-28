using APP.KMX.Entities;
using APP.KMX.Services.Interfaces;
using APP.KMX.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace APP.KMX.Services
{
    public class FileService : IFileService
    {
        public async Task<string> ConvertFileAsync(IFormFile fileData, string uploadFolder)
        {
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }


            if (fileData == null || fileData.Length == 0)
                return string.Empty;

            string fileName = Path.GetFileName(fileData.FileName);
            string fileSavePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(fileSavePath, FileMode.Create))
            {
                await fileData.CopyToAsync(stream);
            }


            return FileConversion.ConvertXlsxToKML(fileSavePath);

        }

    }
}
