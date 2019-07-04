using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DnlForumsData
{
    public interface IUpload
    {
        string UploadImageProfle(IFormFile file);
        void DeleteImageFromFile(string ImageUrl);
    }
}
