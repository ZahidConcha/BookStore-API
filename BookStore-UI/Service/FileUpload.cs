using BlazorInputFile;
using BookStore_UI.Contract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Service
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment env;
        public FileUpload(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public void RemoveFile(string picName)
        {
            var path = $"{env.WebRootPath}\\uploads\\{picName}";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task UploadFile(IFileListEntry file, string picName)
        {
            try
            {
                var ms = new MemoryStream();
                await file.Data.CopyToAsync(ms);

                var path = $"{env.WebRootPath}\\uploads\\{picName}";

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    ms.WriteTo(fs);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public void UploadFile(IFileListEntry file, MemoryStream msFile, string picName)
        {
            try
            {
                var path = $"{env.WebRootPath}\\uploads\\{picName}";

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    msFile.WriteTo(fs);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
