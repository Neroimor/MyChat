namespace ChatApi.Services.ConvertingData
{
    public class ConvertingImage : IConvertingImage
    {
        public Task<string> ConvertImageToBase64(IFormFile file)
        {
            throw new NotImplementedException();
        }
        public Task<IFormFile> ConvertBase64ToImage(string base64String, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> isCheckBase64(string base64String)
        {
            throw new NotImplementedException();
        }
    }

}
