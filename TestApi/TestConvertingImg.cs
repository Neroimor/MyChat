using ChatApi.Services.ConvertingData;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestApi.ProjectDopClass;

namespace TestApi
{
    public class TestConvertingImg
    {
        private readonly IConvertingImage _convertingImage;
        private readonly ManageTestFiles _manageTestFiles;

        public TestConvertingImg()
        {
            _convertingImage = new ConvertingImage();
            _manageTestFiles = new ManageTestFiles();
        }

        [Fact]
        public async Task TestConvertFileToStringAsync()
        {
            var file = _manageTestFiles.readFile();
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
            var file = _manageTestFiles.readFile();
            var result = await _convertingImage.ConvertImageToBase64(file);
            var resultIsCheck = await _convertingImage.ConvertBase64ToImage(result, "notImage.png");
            Assert.NotNull(resultIsCheck);
            Assert.Equal(resultIsCheck.Length, file.Length);
        }

        
    }
}
