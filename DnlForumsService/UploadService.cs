using DnlForumsData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DnlForumsService
{
    public class UploadService : IUpload
    {
        private readonly IHostingEnvironment environment;

        public UploadService(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        public void DeleteImageFromFile(string ImageUrl)
        {
            var fileName = this.environment.WebRootPath + ImageUrl;
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        public string UploadImageProfle(IFormFile file)
        {
            //Getting FileName
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

            //Assigning Unique Filename (Guid)
            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

            //Getting file Extension
            var FileExtension = Path.GetExtension(fileName);

            // concating  FileName + FileExtension
            var newFileName = myUniqueFileName + FileExtension;

            // Combines two strings into a path.
            fileName = Path.Combine(this.environment.WebRootPath, @"images\users") + $@"\{newFileName}";

            // if you want to store path of folder in database
            //string PathDB = "Images/" + newFileName;

            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            return "/images/users/" + newFileName; 
        }
    }
}
