
using Microsoft.Extensions.Primitives;

namespace ChatApi.Services.FileManagment
{
    public class FileManagement : IFileManagement
    {
        public async Task<IFormFile> ReadFileAsync(string filePath)
        {
            var memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await fileStream.CopyToAsync(memoryStream);
            }


            memoryStream.Position = 0;

 
            var fileName = Path.GetFileName(filePath);

            var formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };

            return formFile;
        }
    }
}
