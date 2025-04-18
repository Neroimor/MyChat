using ChatApi.Services.FileManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApi.ProjectDopClass;

namespace TestApi
{
    public class TestFileManagement
    {

        private readonly IFileManagement _fileManagement;
        private readonly ManageTestFiles _testFileManagement;

        public TestFileManagement()
        {
            _fileManagement = new FileManagement();
            _testFileManagement = new ManageTestFiles();
        }

        [Fact]
        public async Task testReadFileAsync()
        {
            // 1) Получаем путь к сборке тестов
            var baseDir = AppContext.BaseDirectory;
            // 2) Поднимаемся на четыре уровня к корню проекта
            var projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", ".."));
            // 3) Строим путь к файлу в папке wwwroot\assets
            var filePath = Path.Combine(projectRoot, "ChatApi", "wwwroot", "assets", "notImage.png");

            // Проверим, что файл действительно существует
            Assert.True(File.Exists(filePath), $"Не найден файл по пути {filePath}");

            // Ваша логика
            var result = await _fileManagement.ReadFileAsync(filePath);

            // Получаем эталонный IFormFile из TestFileManagement
            var expected = _testFileManagement.readFile();
            Assert.NotNull(result);
            Assert.Equal(expected.Length, result.Length);

        }
    }
}
