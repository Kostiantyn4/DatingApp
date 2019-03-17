using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Dto
{
    public class UploadPhotoDto : PhotoDto
    {
        public IFormFile File { get; set; }
        public string PublicId { get; set; }

        public UploadPhotoDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}