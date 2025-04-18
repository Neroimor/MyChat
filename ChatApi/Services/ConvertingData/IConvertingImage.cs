namespace ChatApi.Services.ConvertingData
{
    public interface IConvertingImage
    {
        Task<string> ConvertImageToBase64(IFormFile file);
        Task<IFormFile> ConvertBase64ToImage(string base64String, string fileName);
        Task<bool> IsCheckBase64(string base64String);
    }
}
