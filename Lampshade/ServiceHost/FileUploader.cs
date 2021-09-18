using _0_Framework.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace ServiceHost
{
    public class FileUploader : IFileUploader
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploader(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string Upload(IFormFile file, string path)
        {
            if (file == null) return "";

            var directroyPath = $"{_webHostEnvironment.WebRootPath}//SitePictures//{path}";

            if (!Directory.Exists(directroyPath))
                Directory.CreateDirectory(directroyPath);

            var fileName = $"{DateTime.Now.ToFileName()}-{file.FileName}";
            var filePath = $"{ directroyPath}//{fileName}";

            using var output = System.IO.File.Create(filePath);
            //file.CopyToAsync(output);      
            file.CopyTo(output);

            return $"{path}/{fileName}";
        }
    }
}
