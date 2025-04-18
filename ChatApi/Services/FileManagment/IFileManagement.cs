namespace ChatApi.Services.FileManagment
{
    public interface IFileManagement
    {
        public Task<IFormFile> ReadFileAsync(string filePath);
    }
}
