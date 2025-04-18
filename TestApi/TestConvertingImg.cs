using ChatApi.Services.ConvertingData;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestApi
{
    public class TestConvertingImg
    {
        private readonly IConvertingImage _convertingImage;

        public TestConvertingImg()
        {
            _convertingImage = new ConvertingImage();
        }

        [Fact]
        public async Task TestConvertFileToStringAsync()
        {
            var file = readFile();
            var result = await _convertingImage.ConvertImageToBase64(file);
            var resultIsCheck = await _convertingImage.IsCheckBase64(result);
            Assert.NotNull(result);
            Assert.True(resultIsCheck);
        }

        [Fact]
        public async Task TestCheckBase64Async()
        {
            string data = string.Empty;
            string data2 = "12";
            var result = await _convertingImage.IsCheckBase64(data);
            var result2 = await _convertingImage.IsCheckBase64(data2);
            Assert.False(result);
            Assert.False(result2);
        }

        [Fact]
        public async Task TestConvertStringToFileAsync()
        {
            var file = readFile();
            var result = await _convertingImage.ConvertImageToBase64(file);
            var resultIsCheck = await _convertingImage.ConvertBase64ToImage(result, "notImage.png");
            Assert.NotNull(resultIsCheck);
            Assert.Equal(resultIsCheck.Length, file.Length);
        }

        private IFormFile readFile()
        {
            var assembly = Assembly.GetExecutingAssembly();


            string resourceName = "TestApi.TestFile.notImage.png"; // проверьте полное имя ресурса

            var memorySrteam = new MemoryStream();
            FormFile formFile;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new Exception($"Ресурс '{resourceName}' не найден.");
                }

                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                formFile = new FormFile(memoryStream, 0, memoryStream.Length, null, "notImage.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/octet-stream",
                };

                // Используйте formFile в тесте
            }

            return formFile;
        }
    }
}
