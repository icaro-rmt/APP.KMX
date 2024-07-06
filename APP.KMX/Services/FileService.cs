using APP.KMX.Services.Interfaces;
using APP.KMX.Utils;
using DocumentFormat.OpenXml.Office.Word;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace APP.KMX.Services
{
    public class FileService : IFileService
    {
        public async Task<string> ConvertFileAsync(IFormFile fileData, string uploadFolder)
        {
            try
            {

                await CheckIfFolderExists(uploadFolder);

                if (fileData == null || fileData.Length == 0)
                    return string.Empty;

                string fileName = Path.GetFileName(fileData.FileName);
                string fileSavePath = Path.Combine(uploadFolder, fileName);
                var extension = Path.GetExtension(fileName);

                using (var stream = new FileStream(fileSavePath, FileMode.Create))
                {
                    await fileData.CopyToAsync(stream);
                }

                string convertedFile = SelectFileTypeToConvert(extension, fileSavePath);

                return convertedFile;
            }
            catch (Exception)
            {

                throw new Exception();
            }

        }

        private Task CheckIfFolderExists(string uploadFolder)
        {
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            return Task.CompletedTask;
        }

        //TODO: Clear Files from  Uploads folder after conversion, in order to prevent disk consuming
        public Task ClearFiles(string uploadFolder)
        {
            DirectoryInfo directory = new DirectoryInfo(uploadFolder);

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }

            return Task.CompletedTask;

        }


        private string SelectFileTypeToConvert(string fileType, string fileSavePath)
        { 
            switch (fileType)
            {
                case ".xlsx":
                    return FileConversion.ConvertXlsxToKML(fileSavePath);
                
                case ".kml":
                    return FileConversion.ConvertKMLToXlsx(fileSavePath);

                default: return "";
            }
        }

    }
}
