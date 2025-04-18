using ChatApi.Services.RegisterServices.Interface;

namespace ChatApi.Services.RegisterServices.Realization
{
    public class GeneratorCode : IGenerateCode
    {
        public string GenerateCode(int length)
        {
            int code = new Random().Next(0, length);
            return code.ToString();
        }
    }
}
