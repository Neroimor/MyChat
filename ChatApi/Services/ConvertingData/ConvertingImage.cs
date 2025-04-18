namespace ChatApi.Services.ConvertingData
{
    public class ConvertingImage : IConvertingImage
    {
        public Task<string> ConvertImageToBase64(IFormFile file)
        {
            var base64String = string.Empty;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                base64String = Convert.ToBase64String(fileBytes);
            }

            return Task.FromResult(base64String);
        }

        public Task<IFormFile> ConvertBase64ToImage(string base64String, string fileName)
        {

            var fileBytes = Convert.FromBase64String(base64String);
            var stream = new MemoryStream(fileBytes);

            var formFile = new FormFile(stream, 0, fileBytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };

            return Task.FromResult<IFormFile>(formFile);
        }

        public Task<bool> IsCheckBase64(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return Task.FromResult(false);

            try
            {
                var buffer = Convert.FromBase64String(base64String);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }

}
