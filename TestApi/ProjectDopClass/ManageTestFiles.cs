using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestApi.ProjectDopClass
{
    class ManageTestFiles
    {
        public IFormFile readFile()
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
            }

            return formFile;
        }
    }
}
